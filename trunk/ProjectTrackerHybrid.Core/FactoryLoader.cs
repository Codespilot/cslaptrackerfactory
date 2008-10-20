using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla.Server;

namespace ProjectTracker.Library
{
    public class FactoryLoader : IObjectFactoryLoader
    {
        #region IObjectFactoryLoader Members

        public object GetFactory(string factoryName)
        {
            var ft = Type.GetType(factoryName);
            if (ft == null)
                throw new InvalidOperationException(
                  string.Format("Factory Not Found", factoryName));
            return Activator.CreateInstance(ft);
        }

        public Type GetFactoryType(string factoryName)
        {
            return Type.GetType(factoryName);
        }

        #endregion
    }
}
