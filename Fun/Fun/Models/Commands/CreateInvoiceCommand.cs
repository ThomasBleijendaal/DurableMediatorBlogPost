using MediatR;

namespace Fun.Models.Commands;

internal record CreateInvoiceCommand(string OrderId) : IRequest<Invoice>;
