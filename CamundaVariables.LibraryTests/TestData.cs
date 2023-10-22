using System.Runtime.Serialization;
using System.Xml.Linq;

namespace CamundaVariables.LibraryTests;

public class TestData
{
    public List<string> Tags { get; set; } = new List<string>();
    public bool HasIt {  get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public XDocument ToXDocument()
    {
        var doc = new XDocument();
        using (var writer = doc.CreateWriter())
        {
            var serializer = new DataContractSerializer(GetType());
            serializer.WriteObject(writer, this);
        }
        return doc;
    }

}
