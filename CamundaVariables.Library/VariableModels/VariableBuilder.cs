using System.Text.Json.Nodes;
using System.Xml.Linq;

namespace CamundaVariables.Library.VariableModels;

public class VariableBuilder
{
    private readonly Variables _variables;

    public VariableBuilder()
    {
        _variables = new Variables();
    }

    public VariableBuilder WithVariable(string name, VariableBase variable)
    {
        _variables.Add(name, variable);
        return this;
    }

    public VariableBuilder WithVariable(string name, object? value)
    {
        return WithVariable(name, value switch
        {
            null => new NullVariable(),
            bool => new BooleanVariable((bool)value),
            byte[] => new BytesVariable((byte[])value),
            double => new DoubleVariable((double)value),
            int => new IntegerVariable((int)value),
            JsonNode => new JsonVariable((JsonNode)value),
            long => new LongVariable((long)value),
            short => new ShortVariable((short)value),
            string => new StringVariable((string)value),
            XDocument => new XmlVariable((XDocument)value),
            _ => new ObjectVariable(value)
        });
    }

    public Variables Build()
    {
        return _variables;
    }
}
