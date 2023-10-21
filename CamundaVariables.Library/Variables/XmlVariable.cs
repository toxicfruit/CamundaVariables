using System.Xml.Linq;

namespace CamundaVariables.Library.Variables;

public sealed record XmlVariable(XDocument Value): VariableBase;
