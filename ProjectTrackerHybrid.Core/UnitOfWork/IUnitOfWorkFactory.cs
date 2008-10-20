using NHibernate;
using NHibernate.Cfg;

namespace ProjectTracker.Library
{
    public interface IUnitOfWorkFactory
    {
        Configuration Configuration(string dbKey); // { get; }
        ISessionFactory SessionFactory(string dbKey); // { get; }
        ISession GetCurrentSession(string dbKey);
        void SetCurrentSession(string dbKey, ISession value); //{ get; set; }

        IUnitOfWork Create(string dbKey);
        void DisposeUnitOfWork(UnitOfWorkImplementor adapter, string dbKey);
    }
}