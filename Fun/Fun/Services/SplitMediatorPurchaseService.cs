using System;
using System.Threading.Tasks;
using Fun.Models.Commands;
using Fun.Models.Queries;
using MediatR;

namespace Fun.Services;

public class SplitMediatorPurchaseService
{
    private readonly IMediator _mediator;

    public SplitMediatorPurchaseService(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Guid> StartPurchaseAsync(string orderId)
    {
        var order = await _mediator.Send(new GetOrderQuery(orderId));

        return await _mediator.Send(new CreateInvoiceCommand(order.Id));
    }

    public async Task CompletePurchaseAsync(string orderId, Guid invoiceId)
    {
        var invoice = await _mediator.Send(new GetInvoiceQuery(invoiceId));

        if (invoice.StillProcessing)
        {
            throw new InvalidOperationException("Invoice is still processing");
        }

        var order = await _mediator.Send(new GetOrderQuery(orderId));

        foreach (var product in order.Products)
        {
            await _mediator.Send(new MarkProductAsSoldCommand(product));
        }
    }
}
