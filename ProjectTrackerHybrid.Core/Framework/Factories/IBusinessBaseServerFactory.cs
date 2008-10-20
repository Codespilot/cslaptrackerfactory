using Csla;

namespace ProjectTracker.Library.Framework.Factories
{
    public interface IBusinessBaseServerFactory<T>
    {
        T Create();
        T Fetch();
        T Fetch(CriteriaBase criteria);
        T Update(T obj);
    }
}