using System.Threading.Tasks;
using Fun.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Fun.Example2;

public class Function
{
    private readonly MediatorPurchaseService _purchaseService;

    public Function(MediatorPurchaseService purchaseService)
    {
        _purchaseService = purchaseService;
    }

    [FunctionName("Purchase2")]
    public async Task<IActionResult> PurchaseAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "example2/purchase/{orderId}")] HttpRequest req,
        string orderId)
    {
        await _purchaseService.PurchaseAsync(orderId);

        return new OkResult();
    }
}   
