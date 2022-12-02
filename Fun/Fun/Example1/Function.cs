using System.Threading.Tasks;
using Fun.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Fun.Example1;

public class Function
{
    private readonly PurchaseService _purchaseService;

    public Function(PurchaseService purchaseService)
    {
        _purchaseService = purchaseService;
    }

    [FunctionName("Purchase1")]
    public async Task<IActionResult> PurchaseAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "example1/purchase/{orderId}")] HttpRequest req,
        string orderId)
    {
        await _purchaseService.PurchaseAsync(orderId);

        return new OkResult();
    }
}   
