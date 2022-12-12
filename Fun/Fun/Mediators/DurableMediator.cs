using System.Threading.Tasks;
using Fun.Models.Workflow;
using MediatR;

namespace Fun.Mediators;

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
