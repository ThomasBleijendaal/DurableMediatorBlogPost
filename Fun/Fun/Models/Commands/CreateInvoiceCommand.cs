using System;
using MediatR;

namespace Fun.Models.Commands;

internal record CreateInvoiceCommand(string OrderId) : IRequest<Guid>;

internal record MarkProductAsSoldCommand(Product Product) : IRequest<Unit>;
