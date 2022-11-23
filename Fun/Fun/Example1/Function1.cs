using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Fun.Example1
{
    public class Function1
    {
        private readonly PurchaseService _purchaseService = new PurchaseService();

        [FunctionName("Purchase")]
        public async Task<IActionResult> PurchaseAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "example1/purchase/{orderId}")] HttpRequest req,
            string orderId)
        {
            await _purchaseService.PurchaseAsync(orderId);

            return new OkResult();
        }
    }

    public class PurchaseService
    {
        private readonly OrderGateway _orderGateway = new OrderGateway();

        public async Task PurchaseAsync(string orderId)
        {
            var order = await _orderGateway.GetOrderAsync(orderId);

            if (order == null)
            {
                return;
            }

            await _orderGateway.CreateInvoiceAsync(orderId);

            await _orderGateway.MarkOrderAsSoldAsync(orderId);
        }
    }

    public class OrderGateway
    {
        public Task<Order?> GetOrderAsync(string id) => Task.FromResult(new Order("1", new List<Product> { new Product("1") }))!;
        public Task CreateInvoiceAsync(string orderId) => Task.CompletedTask;
        public Task MarkOrderAsSoldAsync(string orderId) => Task.CompletedTask;

    }

    public record Product(string Id);

    public record Order(string Id, List<Product> Products);
}
