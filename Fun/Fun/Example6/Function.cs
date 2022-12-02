using System;
using System.Threading;
using System.Threading.Tasks;
using Fun.Models.Commands;
using Fun.Models.Queries;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json;
using static Fun.Example6.Function;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace Fun.Example6;

public class Function
{
    private readonly IMediator _mediator;

    public Function(
        IMediator mediator)
    {
        _mediator = mediator;
    }   

    

    [FunctionName(nameof(DurableMediator))]
    public async Task RunAsync([EntityTrigger] IDurableEntityContext ctx) => await ctx.DispatchAsync<DurableMediator>();

    

    [FunctionName("PurchaseWorkflowOrchestrator")]
    public async Task OrchestrateAsync(
        [OrchestrationTrigger] IDurableOrchestrationContext context)
    {
        var workflow = new PurchaseWorkflow();

        await workflow.OrchestrateAsync(context, new DurableMediator(_mediator));
    }
}
public interface IDurableMediator
{
    Task SendObjectAsync(WorkflowRequest request);
    Task<WorkflowResponse> SendObjectWithResponseAsync(WorkflowRequestWithResponse request);
}
public class DurableMediator : IDurableMediator
{
    private readonly IMediator _mediator;

    public DurableMediator(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task SendObjectAsync(WorkflowRequest request)
    {
        await _mediator.Send(request.Request);
    }

    public async Task<WorkflowResponse> SendObjectWithResponseAsync(WorkflowRequestWithResponse request)
    {
        // the dynamic is needed for the dynamic dispatch of Send()
        var result = await _mediator.Send(request.Request);
        return new WorkflowResponse(result);
    }
}

[JsonObject(ItemTypeNameHandling = TypeNameHandling.All)]
public record WorkflowRequest(IRequest<Unit> Request);

[JsonObject(ItemTypeNameHandling = TypeNameHandling.All)]
public record WorkflowRequestWithResponse(dynamic Request);

[JsonObject(ItemTypeNameHandling = TypeNameHandling.All)]
public record WorkflowResponse(object Response);

public class PurchaseWorkflow
{
    public async Task OrchestrateAsync(IDurableOrchestrationContext context, IDurableMediator mediator)
    {
        var orderId = context.GetInput<string>();

        var order = await mediator.SendAsync(new GetOrderQuery(orderId));

        var invoiceId = await mediator.SendAsync(new CreateInvoiceCommand(order.Id));

        do
        {
            var invoice = await mediator.SendAsync(new GetInvoiceQuery(invoiceId));

            if (!invoice.StillProcessing)
            {
                break;
            }

            await context.CreateTimer(context.CurrentUtcDateTime.AddSeconds(1), CancellationToken.None);
        }
        while (true);

        foreach (var product in order.Products)
        {
            await mediator.SendAsync(new MarkProductAsSoldCommand(product));
        }
    }
}

public static class DurableMediatorExtensions
{
    public static async Task<TResponse> SendAsync<TResponse>(this IDurableMediator mediator, IRequest<TResponse> request)
    {
        if (typeof(TResponse) == typeof(Unit))
        {
            await mediator.SendObjectAsync(new WorkflowRequest((IRequest<Unit>)request));

            return default!;
        }

        var response = await mediator.SendObjectWithResponseAsync(new WorkflowRequestWithResponse((IRequest<object>)request));

        if (response == null)
        {
            throw new InvalidOperationException("Received an empty response");
        }

        return (TResponse)(response.Response);
    }
}
