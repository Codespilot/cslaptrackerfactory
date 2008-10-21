using FluentNHibernate.Mapping;
using ProjectTracker.Library.Admin;

namespace ProjectTracker.Library.Mapping
{
    public class RoleMap : PTClassMap<Role>
    {
        public RoleMap()
        {
            WithTable("Roles");

            Id(x => x.Id)
                .GeneratedBy.Assigned();

            Map(x => x.Name)
                .WithLengthOf(50);
        }
    }

    public class RoleSubMap : PTClassMap<RoleNV>
    {
        public RoleSubMap()
        {

            WithTable("Roles");

            Id(x => x.Id)
                .GeneratedBy.Assigned();

            Map(x => x.Name)
                .WithLengthOf(50);
        }
    }

   

    
}
