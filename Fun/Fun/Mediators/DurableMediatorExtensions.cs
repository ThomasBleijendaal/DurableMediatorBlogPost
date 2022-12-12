using System;
using System.Threading.Tasks;
using Fun.Models.Workflow;
using MediatR;

namespace Fun.Mediators;

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

        return (TResponse)response.Response;
    }
}
