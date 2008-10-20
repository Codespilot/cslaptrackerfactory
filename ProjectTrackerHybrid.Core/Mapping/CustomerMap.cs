using FluentNHibernate.Mapping;

namespace ProjectTracker.Library.Mapping
{


    public class CustomerMap : ClassMap<Customer>
    {
        public CustomerMap()
        {
            WithTable("Customer");

            Id(x => x.Id)
                .GeneratedBy.GuidComb()
                 .WithUnsavedValue("00000000-0000-0000-0000-000000000000");

            Map(x => x.FirstName)
                .CanNotBeNull()
                .WithLengthOf(50);

            Map(x => x.LastName)
                .CanNotBeNull()
                .WithLengthOf(50);


        }
    }
}
