using MediatR;
using Newtonsoft.Json;

namespace Fun.Models.Workflow;

[JsonObject(ItemTypeNameHandling = TypeNameHandling.All)]
public record WorkflowRequest(IRequest<Unit> Request);
