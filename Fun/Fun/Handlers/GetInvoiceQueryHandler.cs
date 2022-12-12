using System;
using System.Threading;
using System.Threading.Tasks;
using Fun.Models;
using Fun.Models.Queries;
using MediatR;

namespace Fun.Handlers;

internal record GetInvoiceQueryHandler : IRequestHandler<GetInvoiceQuery, Invoice>
{
    public async Task<Invoice> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
    {
        await Task.Delay(2000);

        return new Invoice(request.InvoiceId, Random.Shared.NextDouble() < 0.5);
    }
}
