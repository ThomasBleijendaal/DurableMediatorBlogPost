using System;
using System.Threading;
using System.Threading.Tasks;
using Fun.Models;
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
