using System;
using MediatR;

namespace Fun.Models.Queries;

internal record GetOrderQuery(string OrderId) : IRequest<Order>;

internal record GetInvoiceQuery(Guid InvoiceId) : IRequest<Invoice>;
