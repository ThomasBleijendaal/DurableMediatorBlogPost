using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace Fun.Services;

public class WorkflowPurchaseService
{
    private readonly IDurableClient _durableClient;

    public WorkflowPurchaseService(
        IDurableClient durableClient)
    {
        _durableClient = durableClient;
    }

    public async Task<string> PurchaseAsync(string orderId)
    {
        return await _durableClient.StartNewAsync("PurchaseWorkflowOrchestrator", input: orderId);
    }
}
