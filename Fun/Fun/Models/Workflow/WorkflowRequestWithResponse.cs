using Newtonsoft.Json;

namespace Fun.Models.Workflow;

[JsonObject(ItemTypeNameHandling = TypeNameHandling.All)]
public record WorkflowRequestWithResponse(dynamic Request);
