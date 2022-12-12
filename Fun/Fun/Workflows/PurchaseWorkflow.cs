using System.Threading;
using System.Threading.Tasks;
using Fun.Mediators;
using Fun.Models.Commands;
using Fun.Models.Queries;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace Fun.Workflows;

public class PurchaseWorkflow
{
    public async Task OrchestrateAsync(IDurableOrchestrationContext context)
    {
        var orderId = context.GetInput<string>();
        var mediator = context.CreateEntityProxy<IDurableMediator>(new EntityId(nameof(DurableMediator), context.InstanceId));

        var order = await mediator.SendAsync(new GetOrderQuery(orderId));

        var createdInvoice = await mediator.SendAsync(new CreateInvoiceCommand(order.Id));

        do
        {
            var invoice = await mediator.SendAsync(new GetInvoiceQuery(createdInvoice.Id));

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
