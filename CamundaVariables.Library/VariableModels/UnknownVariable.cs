using System.Text.Json.Nodes;

namespace CamundaVariables.Library.VariableModels;

public sealed record UnknownVariable(string Type, JsonNode? Value) : VariableBase;
