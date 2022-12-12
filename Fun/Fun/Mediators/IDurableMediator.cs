using System.Threading.Tasks;
using Fun.Models.Workflow;

namespace Fun.Mediators;

public interface IDurableMediator
{
    Task SendObjectAsync(WorkflowRequest request);
    Task<WorkflowResponse> SendObjectWithResponseAsync(WorkflowRequestWithResponse request);
}
