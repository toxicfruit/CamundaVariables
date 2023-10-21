using System.Xml.Linq;

namespace CamundaVariables.Library.VariableModels;

public sealed record XmlVariable(XDocument Value) : VariableBase;
