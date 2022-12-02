using System;
using System.Threading.Tasks;
using Fun.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Fun.Example3;

public class Function
{
    private readonly SplitMediatorPurchaseService _purchaseService;

    public Function(SplitMediatorPurchaseService purchaseService)
    {
        _purchaseService = purchaseService;
    }

    [FunctionName("Purchase3")]
    public async Task<IActionResult> PurchaseAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "example3/purchase/{orderId}")] HttpRequest req,
        [DurableClient] IDurableOrchestrationClient orchestrationClient,
        string orderId)
    {
        var invoiceId = await _purchaseService.StartPurchaseAsync(orderId);

        await orchestrationClient.StartNewAsync("Purchase3Orchestrator", new Purchase(orderId, invoiceId));

        return new StatusCodeResult(StatusCodes.Status202Accepted);
    }

    [FunctionName("Purchase3Orchestrator")]
    public async Task OrchestrateAsync(
        [OrchestrationTrigger]IDurableOrchestrationContext context)
    {
        await context.CallActivityWithRetryAsync("Purchase3Activity", new RetryOptions(TimeSpan.FromSeconds(1), 10), context.GetInput<Purchase>());
    }

    [FunctionName("Purchase3Activity")]
    public async Task ActivityAsync(
        [ActivityTrigger]Purchase purchase)
    {
        await _purchaseService.CompletePurchaseAsync(purchase.OrderId, purchase.InvoiceId);
    }

    public record Purchase(string OrderId, Guid InvoiceId);
}   
