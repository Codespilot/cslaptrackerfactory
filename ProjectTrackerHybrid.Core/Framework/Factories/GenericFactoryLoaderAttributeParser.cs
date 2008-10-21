using ProjectTracker.Library.Framework.Factories;

namespace ProjectTracker.Library.Framework.Factories
{
    public class GenericFactoryLoaderAttributeParser : IFactoryLoaderConfigParser
    {

        private string _factoryType;
        private string _listType;
        private string _itemType;
        private string _dtoType;
        private string _keyType;
        private string _valueType;

        public string KeyType
        {
            get { return _keyType; }
        }

        public string ValueType
        {
            get { return _valueType; }
        }

        public string FactoryType
        {
            get { return _factoryType; }
        }

        public string ListType
        {
            get { return _listType; }
        }

        public string ItemType
        {
            get { return _itemType; }
        }

        public string DTOType
        {
            get { return _dtoType; }
        }

        public string RepositoryType
        {
            get { throw new System.NotImplementedException("Repository Type on Object Factory Attribute is not supported yet"); }
        }

        public void Parse(string @string)
        {
            string[] values = @string.Split(';');

            foreach (var s in values)
            {
                string[] value = s.Split('=');

                switch (value[0])
                {
                    case "Factory Type":
                        _factoryType = value[1];
                        break;
                    case "List Type":
                        _listType = value[1];
                        break;
                    case "Item Type":
                        _itemType = value[1];
                        break;
                    case "DTO Type":
                        _dtoType = value[1];
                        break;
                    case "Key Type":
                        _keyType = value[1];
                        break;
                    case "Value Type":
                        _valueType = value[1];
                        break;

                }
                
            }
            
            
        }
    }
}