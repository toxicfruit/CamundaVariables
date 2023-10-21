using System.Text.Json.Nodes;

namespace CamundaVariables.Library.Variables;

public sealed record UnknownVariable(string Type, JsonNode? Value): VariableBase;
