using Csla;

namespace ProjectTracker.Library.Framework.Factories
{
    public interface IReadOnlyBaseServerFactory<T>
    {
        T Create();
        T Fetch();
        T Fetch(CriteriaBase criteria);
    }
}