using System;
using System.Threading;
using System.Threading.Tasks;
using Fun.Models;
using Fun.Models.Commands;
using Fun.Models.Queries;
using Fun.Services;
using MediatR;

namespace Fun.Handlers;

internal record GetOrderQueryHandler(OrderGateway OrderGateway) : IRequestHandler<GetOrderQuery, Order>
{
    public async Task<Order> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        return await OrderGateway.GetOrderAsync(request.OrderId) ?? throw new InvalidOperationException("Order not found");
    }
}

internal record CreateInvoiceCommandHandler(OrderGateway OrderGateway) : IRequestHandler<CreateInvoiceCommand, Guid>
{
    public Task<Guid> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        return OrderGateway.CreateInvoiceAsync(request.OrderId);
    }
}

internal record MarkProductAsSoldCommandHandler(OrderGateway OrderGateway) : IRequestHandler<MarkProductAsSoldCommand>
{
    public async Task<Unit> Handle(MarkProductAsSoldCommand request, CancellationToken cancellationToken)
    {
        await OrderGateway.MarkProductAsSoldAsync(request.Product);

        return Unit.Value;
    }
}

internal record GetInvoiceQueryHandler : IRequestHandler<GetInvoiceQuery, Invoice>
{
    public async Task<Invoice> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
    {
        await Task.Delay(2000);

        return new Invoice(request.InvoiceId, Random.Shared.NextDouble() < 0.5);
    }
}
