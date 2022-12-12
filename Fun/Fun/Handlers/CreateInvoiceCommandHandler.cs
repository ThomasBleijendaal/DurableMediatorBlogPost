using System.Threading;
using System.Threading.Tasks;
using Fun.Models;
using Fun.Models.Commands;
using Fun.Services;
using MediatR;

namespace Fun.Handlers;

internal record CreateInvoiceCommandHandler(OrderGateway OrderGateway) : IRequestHandler<CreateInvoiceCommand, Invoice>
{
    public async Task<Invoice> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        return new Invoice(await OrderGateway.CreateInvoiceAsync(request.OrderId), true);
    }
}
