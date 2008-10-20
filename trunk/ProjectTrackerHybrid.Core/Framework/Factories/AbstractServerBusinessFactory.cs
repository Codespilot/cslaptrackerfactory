using Csla;
using Csla.Server;

namespace ProjectTracker.Library.Framework.Factories
{
    public abstract class AbstractServerBusinessFactory<T> : ObjectFactory
    {
        public abstract T Create(SingleCriteria<T, int> criteria);
        public abstract T Fetch();
        public abstract T Fetch(CriteriaBase criteria);
        public abstract T Create();
    }
}