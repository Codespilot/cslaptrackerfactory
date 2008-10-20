using Csla;

namespace ProjectTracker.Library.Framework.Factories
{
    public interface INameValueListServerFactory<T,K,V,I>
    {

        T Create();
        T Fetch();
        T Fetch(CriteriaBase criteria);
    }
}