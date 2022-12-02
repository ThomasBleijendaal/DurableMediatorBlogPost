using System.Threading.Tasks;

namespace Fun.Services;

public class PurchaseService
{
    private readonly OrderGateway _orderGateway;

    public PurchaseService(
        OrderGateway orderGateway)
    {
        _orderGateway = orderGateway;
    }

    public async Task PurchaseAsync(string orderId)
    {
        var order = await _orderGateway.GetOrderAsync(orderId);

        if (order == null)
        {
            return;
        }

        await _orderGateway.CreateInvoiceAsync(orderId);

        foreach (var product in order.Products)
        {
            await _orderGateway.MarkProductAsSoldAsync(product);
        }
    }
}
