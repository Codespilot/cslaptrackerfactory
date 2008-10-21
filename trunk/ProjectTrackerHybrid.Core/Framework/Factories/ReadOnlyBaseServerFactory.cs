using System;
using Csla;
using ProjectTracker.Library.Data;
using ProjectTracker.Library.Framework.Factories;

namespace ProjectTracker.Library.Framework.Factories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Csla ReadOnlybase type</typeparam>
    public class ReadOnlyBaseServerFactory<T> :
        AbstractServerBusinessFactory<T>, IReadOnlyBaseServerFactory<T>
        where T : PTReadOnlyBase<T>
    {
        private readonly IRepository<T> _repository;

        private string DatabaseKey { get { return DatabaseKeyAttribute.GetDatabaseKeyForClass(typeof(T)); } }

        public ReadOnlyBaseServerFactory(IRepository<T> repository)
        {
            _repository = repository;
        }

        public override T Create()
        {
            var obj = Activator.CreateInstance(typeof(T), true);
            MarkNew(obj);
            return (T)obj;
        }

        public override T Create(SingleCriteria<T, int> criteria)
        {
            var obj = Activator.CreateInstance(typeof(T), true);
            MarkNew(obj);
            return (T)obj;
        }

        public override T Fetch()
        {
            throw new NotImplementedException("You must pass critiria to load a Root Object");
        }


        public override T Fetch(CriteriaBase criteria)
        {
            if (criteria != null)
            {
                var obj = (T) Activator.CreateInstance(typeof (T), true);

                using (UnitOfWork.Start(DatabaseKey))
                {
                    object crit = obj.GetObjectCriteriaValue(criteria);

                    _repository.Load(obj, crit);
                }
                MarkOld(obj);
                return obj;
            }
            return Fetch();
        }
    }
}