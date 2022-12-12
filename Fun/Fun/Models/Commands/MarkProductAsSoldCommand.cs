using MediatR;

namespace Fun.Models.Commands;

internal record MarkProductAsSoldCommand(Product Product) : IRequest<Unit>;
