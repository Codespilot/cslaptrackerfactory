using System.Xml;

namespace ProjectTracker.Library.Mapping.Helpers
{
    public interface IMapGenerator
    {
        string FileName { get; }
        XmlDocument Generate();
    }
}
