using Csla;
using ProjectTracker.Library.Data;
using ProjectTracker.Library.Framework;

namespace ProjectTracker.Library.Tests.FactoryFixtures
{
    [DatabaseKey("TEST")]
    public class Product : PTBusinessBase<Product>
    {
        private static PropertyInfo<string> NameProperty = RegisterProperty<string>(typeof(Product), new PropertyInfo<string>("Name", "Name"));
        public string Name
        {
            get { return GetProperty<string>(NameProperty); }
            set { SetProperty<string>(NameProperty, value); }
        }

        public override object GetObjectCriteriaValue(object businessCriteria)
        {
            // Cast the criteria back to the strongly-typed version
            SingleCriteria<Product, int> criteria = businessCriteria as SingleCriteria<Product, int>;

            // If it's a valid criteria object then check for filters
            if (!ReferenceEquals(criteria, null))
            {
                // Set a reference to the NHibernate ICriteria (for local use only)
                //_iCriteria = nhibernateCriteria;
                return criteria.Value;
            }
            return null;
        }

        public new void MarkDeleted()
        {
            base.MarkDeleted();
        }

        public new void MarkOld()
        {
            base.MarkOld();
        }

        public new void MarkAsChild()
        {
            base.MarkAsChild();
        }

        public static Product GetOldProduct()
        {
            Product product = new Product();
            product.MarkOld();
            return product;
        }
    }
}