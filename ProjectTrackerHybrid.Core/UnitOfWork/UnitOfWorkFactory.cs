using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using NHibernate;
using NHibernate.Cfg;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using System.Reflection;

namespace ProjectTracker.Library
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private static Dictionary<string, ISession> _sessions = new Dictionary<string, ISession>();
        private Dictionary<string, ISessionFactory> _sessionFactories = new Dictionary<string, ISessionFactory>();
        private Dictionary<string, Configuration> _configurations = new Dictionary<string, Configuration>();
        internal UnitOfWorkFactory()
        { }

        public IUnitOfWork Create(string dbKey)
        {
            ISession session = CreateSession(dbKey);
            session.FlushMode = FlushMode.Commit;
            _sessions[dbKey] = session;
            return new UnitOfWorkImplementor(this, session, dbKey);
        }

        public Configuration Configuration(string dbKey)
        {
            if (!_configurations.ContainsKey(dbKey) || _configurations[dbKey] == null)
            {
                IPersistenceConfigurer pConfigurer = MsSqlConfiguration
                    .MsSql2005
                    .ConnectionString.Is(
                    System.Configuration.ConfigurationManager.ConnectionStrings[dbKey].ConnectionString)
                    .ShowSql().UseReflectionOptimizer();

                _configurations[dbKey] = pConfigurer.ConfigureProperties(new Configuration());
                
                // Not needed with NH 2.0
                //_configurations[dbKey].Properties.Add(new KeyValuePair<string, string>("proxyfactory.factory_class", "NHibernate.ProxyGenerators.CastleDynamicProxy.ProxyFactoryFactory, NHibernate.ProxyGenerators.CastleDynamicProxy"));
                var persistenceModel = new PersistenceModel();

                
                persistenceModel.addMappingsFromAssembly(
                    Assembly.Load("ProjectTracker.Library"));
                persistenceModel.Configure(_configurations[dbKey]);
                string test = _configurations[dbKey].ToString();
                _configurations[dbKey] = _configurations[dbKey];

            }
            return _configurations[dbKey];
        }

        public ISessionFactory SessionFactory(string dbKey)
        {
            if (!_sessionFactories.ContainsKey(dbKey) || _sessionFactories[dbKey] == null)
                _sessionFactories[dbKey] = Configuration(dbKey).BuildSessionFactory();

            return _sessionFactories[dbKey];
        }

        public ISession GetCurrentSession(string dbKey)
        {

            if (!_sessions.ContainsKey(dbKey) || _sessions[dbKey] == null)
                throw new InvalidOperationException("You are not in a unit of work.");
            return _sessions[dbKey];
        }

        public void SetCurrentSession(string dbKey, ISession value)
        {
            _sessions[dbKey] = value;
        }
        

        public void DisposeUnitOfWork(UnitOfWorkImplementor adapter, string dbKey)
        {
            SetCurrentSession(dbKey, null);
            UnitOfWork.DisposeUnitOfWork(adapter);
        }

        private ISession CreateSession(string dbKey)
        {
            return SessionFactory(dbKey).OpenSession();
        }
    }
}