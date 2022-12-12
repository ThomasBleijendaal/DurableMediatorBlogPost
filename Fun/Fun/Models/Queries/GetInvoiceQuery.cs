using System;
using MediatR;

namespace Fun.Models.Queries;

internal record GetInvoiceQuery(Guid InvoiceId) : IRequest<Invoice>;
