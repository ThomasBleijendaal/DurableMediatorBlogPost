using System.Threading.Tasks;
using Fun.Models.Commands;
using Fun.Models.Queries;
using MediatR;

namespace Fun.Services;

public class MediatorPurchaseService
{
    private readonly IMediator _mediator;

    public MediatorPurchaseService(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task PurchaseAsync(string orderId)
    {
        var order = await _mediator.Send(new GetOrderQuery(orderId));

        await _mediator.Send(new CreateInvoiceCommand(orderId));

        foreach (var product in order.Products)
        {
            await _mediator.Send(new MarkProductAsSoldCommand(product));
        }
    }
}
