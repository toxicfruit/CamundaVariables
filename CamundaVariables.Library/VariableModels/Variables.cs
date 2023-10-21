using System.Collections.Generic;

namespace CamundaVariables.Library.VariableModels;

public class Variables : Dictionary<string, VariableBase>
{
    public Variables WithVariable(string variableName, VariableBase variable)
    {
        this[variableName] = variable;
        return this;
    }
}
