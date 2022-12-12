using System.Threading;
using System.Threading.Tasks;
using Fun.Models.Commands;
using Fun.Services;
using MediatR;

namespace Fun.Handlers;

internal record MarkProductAsSoldCommandHandler(OrderGateway OrderGateway) : IRequestHandler<MarkProductAsSoldCommand>
{
    public async Task<Unit> Handle(MarkProductAsSoldCommand request, CancellationToken cancellationToken)
    {
        await OrderGateway.MarkProductAsSoldAsync(request.Product);

        return Unit.Value;
    }
}
