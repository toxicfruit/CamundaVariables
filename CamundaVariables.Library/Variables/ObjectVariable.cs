using System.Collections.Generic;
using System.Net.Mime;

namespace CamundaVariables.Library.Variables;

public sealed record ObjectVariable(object Value): VariableBase;
//{
//    public static IDictionary<string, object> ValueInfo => new Dictionary<string, object>
//    {
//        ["serializationDataFormat"] = MediaTypeNames.Application.Json,
//        ["objectTypeName"] = "java.lang.Object"
//    };
//}
