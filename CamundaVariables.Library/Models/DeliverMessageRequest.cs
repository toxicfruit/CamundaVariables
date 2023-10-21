using CamundaVariables.Library.VariableModels;

namespace CamundaVariables.Library.Models;

public sealed class DeliverMessageRequest
{
    public string MessageName { get; set; } = string.Empty;
    public string? BusinessKey { get; set; }
    public string? TenantId { get; set; }
    public bool? WithoutTenantId { get; set; }
    public string? ProcessInstanceId {  get; set; }
    public Variables? CorrelationKeys { get; set; }
    public Variables? ProcessVariables { get; set; }
    public bool All {  get; set; }
}
