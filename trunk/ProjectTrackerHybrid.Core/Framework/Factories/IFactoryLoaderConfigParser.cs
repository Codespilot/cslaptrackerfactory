namespace ProjectTracker.Library.Framework.Factories
{
    public interface IFactoryLoaderConfigParser
    {
        string FactoryType { get; }
        string ListType { get; }
        string ItemType { get; }
        string DTOType { get; }
        string KeyType { get;}
        string ValueType { get; }
        string RepositoryType { get; }
        void Parse(string @string);
    }
}