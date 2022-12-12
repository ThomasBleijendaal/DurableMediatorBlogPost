using MediatR;

namespace Fun.Models.Queries;

internal record GetOrderQuery(string OrderId) : IRequest<Order>;
