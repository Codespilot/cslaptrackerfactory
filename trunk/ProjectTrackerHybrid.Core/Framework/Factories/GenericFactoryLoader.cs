using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla.Server;
using ProjectTracker.Library.Framework.Factories;
using StructureMap;

namespace ProjectTracker.Library.Framework.Factories
{
    public class GenericFactoryLoader : IObjectFactoryLoader
    {
        #region Constructor

        private readonly IFactoryLoaderConfigParser parser;

        public GenericFactoryLoader()
            : this(new GenericFactoryLoaderAttributeParser())
        {
        }
        public GenericFactoryLoader(IFactoryLoaderConfigParser parser)
        {
            this.parser = parser;
        }

        #endregion

        public virtual Type GetFactoryType(string factoryConnectionString)
        {
            if (factoryConnectionString == null)
                throw new ArgumentNullException("factoryConnectionString");

            parser.Parse(factoryConnectionString);

            var factoryTypeName = parser.FactoryType;

            if (factoryTypeName == "IReadOnlyListServerFactory")
            {
                var generic = typeof(IReadOnlyListServerFactory<,>);
                Type[] typeArgs = { Type.GetType(parser.ListType), Type.GetType(parser.ItemType) };
                var constructed = generic.MakeGenericType(typeArgs);
                return constructed;
            }
            else if (factoryTypeName == "IBusinessBaseServerFactory")
            {
                var generic = typeof(IBusinessBaseServerFactory<>);
                Type[] typeArgs = { Type.GetType(parser.ItemType) };
                var constructed = generic.MakeGenericType(typeArgs);
                return constructed;

            }
            else if (factoryTypeName == "INameValueListServerFactory")
            {
                var generic = typeof(INameValueListServerFactory<,,,>);
                Type[] typeArgs = { Type.GetType(parser.ListType), Type.GetType(parser.KeyType), Type.GetType(parser.ValueType), Type.GetType(parser.ItemType) };
                var constructed = generic.MakeGenericType(typeArgs);
                return constructed;
            }
            else if(factoryTypeName == "IBusinessListBaseServerFactory")
            {
                var generic = typeof (IBusinessListBaseServerFactory<,>);
                Type[] typeArgs = {Type.GetType(parser.ListType), Type.GetType(parser.ItemType)};
                var constructed = generic.MakeGenericType(typeArgs);
                return constructed;
            }
            else if(factoryTypeName == "IReadOnlyBaseServerFactory")
            {
                var generic = typeof(IReadOnlyBaseServerFactory<>);
                Type[] typeArgs = { Type.GetType(parser.ItemType) };
                var constructed = generic.MakeGenericType(typeArgs);
                return constructed;
            }

            throw new UnkownFactoryTypeArgumentException();

        }

        public virtual object GetFactory(string factoryName)
        {
            var factoryType = GetFactoryType(factoryName);

            if (factoryType == null)
                throw new InvalidOperationException(
                    string.Format("Factory Type Not Found: {0}", factoryName));

            //if (factoryType.IsGenericType)
            //{
                if (factoryType.GetGenericTypeDefinition() == typeof (IReadOnlyListServerFactory<,>))
                {
                    var generic = typeof (IReadOnlyListServerFactory<,>);
                    Type[] typeArgs = {Type.GetType(parser.ListType), Type.GetType(parser.ItemType)};
                    return StructureMap.ObjectFactory.GetInstance(generic.MakeGenericType(typeArgs));

                }
                else if (factoryType.GetGenericTypeDefinition() == typeof (IBusinessBaseServerFactory<>))
                {
                    var generic = typeof (IBusinessBaseServerFactory<>);
                    Type[] typeArgs = {Type.GetType(parser.ItemType)};
                    return StructureMap.ObjectFactory.GetInstance(generic.MakeGenericType(typeArgs));
                }
                else if(factoryType.GetGenericTypeDefinition() == typeof(INameValueListServerFactory<,,,>))
                {
                    var generic = typeof(INameValueListServerFactory<,,,>);
                    Type[] typeArgs = { Type.GetType(parser.ListType), Type.GetType(parser.KeyType), Type.GetType(parser.ValueType), Type.GetType(parser.ItemType) };
                    return StructureMap.ObjectFactory.GetInstance(generic.MakeGenericType(typeArgs));
                }
                else if(factoryType.GetGenericTypeDefinition() == typeof(IBusinessListBaseServerFactory<,>))
                {
                    var generic = typeof (IBusinessListBaseServerFactory<,>);
                    Type[] typeArgs = {Type.GetType(parser.ListType), Type.GetType(parser.ItemType)};
                    return StructureMap.ObjectFactory.GetInstance(generic.MakeGenericType(typeArgs));
                }
                else if(factoryType.GetGenericTypeDefinition() == typeof(IReadOnlyBaseServerFactory<>))
                {
                    var generic = typeof (IReadOnlyBaseServerFactory<>);
                    Type[] typeArgs = {Type.GetType(parser.ItemType)};
                    return StructureMap.ObjectFactory.GetInstance(generic.MakeGenericType(typeArgs));
                }
            //}
            //else
            //{
                //// This piece of code is used for unit testing, until we find a better way.
                //if (factoryType.Name.Contains("IReadOnlyListServerFactory"))
                //{
                //    var generic = typeof(IReadOnlyListServerFactory<,>);
                //    Type[] typeArgs = { Type.GetType(parser.ListType), Type.GetType(parser.ItemType) };
                //    return StructureMap.ObjectFactory.GetInstance(generic.MakeGenericType(typeArgs));
                //}
                //else if (factoryType.Name.Contains("IBusinessBaseServerFactory"))
                //{
                //    var generic = typeof(IBusinessBaseServerFactory<>);
                //    Type[] typeArgs = { Type.GetType(parser.ItemType) };
                //    return StructureMap.ObjectFactory.GetInstance(generic.MakeGenericType(typeArgs));
                //}
                //else if (factoryType.Name.Contains("INameValueListServerFactory"))
                //{
                //    var generic = typeof(INameValueListServerFactory<,,,>);
                //    Type[] typeArgs = { Type.GetType(parser.ListType), Type.GetType(parser.KeyType), Type.GetType(parser.ValueType), Type.GetType(parser.ItemType) };
                //    return StructureMap.ObjectFactory.GetInstance(generic.MakeGenericType(typeArgs));
                //}
                //else if (factoryType.Name.Contains("IBusinessListBaseServerFactory"))
                //{
                //    var generic = typeof(IBusinessListBaseServerFactory<,>);
                //    Type[] typeArgs = { Type.GetType(parser.ListType), Type.GetType(parser.ItemType) };
                //    return StructureMap.ObjectFactory.GetInstance(generic.MakeGenericType(typeArgs));
                //}
                //else if (factoryType.Name.Contains("IReadOnlyBaseServerFactory"))
                //{
                //    var generic = typeof(IReadOnlyBaseServerFactory<>);
                //    Type[] typeArgs = { Type.GetType(parser.ItemType) };
                //    return StructureMap.ObjectFactory.GetInstance(generic.MakeGenericType(typeArgs));
                //}
            //}
            throw new UnkownFactoryTypeArgumentException();

        }
    }
}