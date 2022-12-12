using System.Threading.Tasks;
using Fun.Mediators;
using Fun.Services;
using Fun.Workflows;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Fun.Example6;

public class Function
{
    private readonly WorkflowPurchaseService _purchaseService;

    public Function(
        WorkflowPurchaseService purchaseService)
    {
        _purchaseService = purchaseService;
    }

    [FunctionName("Purchase6")]
    public async Task<IActionResult> PurchaseAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "example6/purchase/{orderId}")] HttpRequest req,
        string orderId)
    {
        var instanceId = await _purchaseService.PurchaseAsync(orderId);
        return new AcceptedResult(string.Empty, instanceId);
    }

    [FunctionName("PurchaseWorkflowOrchestrator")]
    public async Task OrchestrateAsync(
        [OrchestrationTrigger] IDurableOrchestrationContext context)
    {
        var purchaseWorkflow = new PurchaseWorkflow();
        await purchaseWorkflow.OrchestrateAsync(context);
    }

    [FunctionName(nameof(DurableMediator))]
    public static async Task RunAsync([EntityTrigger] IDurableEntityContext ctx) 
        => await ctx.DispatchAsync<DurableMediator>();
}
