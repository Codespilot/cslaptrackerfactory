using FluentNHibernate.Mapping;
using ProjectTracker.Library.Admin;
using ProjectTracker.Library.Security;


namespace ProjectTracker.Library.Mapping
{
    //public class UserMap : PTClassMap<User>
    //{
    //    public UserMap()
    //    {

    //        WithTable("Users");

    //        Id(x => x.Username)
    //            .GeneratedBy.Assigned();

    //        //Map(x => x.Username)
    //        //    .WithLengthOf(50);

    //        Map(x => x.Password)
    //            .WithLengthOf(50);
            
    //        HasMany<Role>(x => x.RolesSet)
    //            .AsBag()
    //            .Access.AsProperty()
    //            .Cascade.All()
    //            .WithKeyColumn("Username");

    //    }
    //}

    //public class SecurityRoleMap : PTClassMap<Admin.Role>
    //{
    //    public SecurityRoleMap()
    //    {
    //        WithTable("Roles");

    //        Map(x => x.Name);
    //    }

    //}

    public class PTIdentityMap : PTClassMap<PTIdentity>
    {

        public PTIdentityMap()
        {
            WithTable("Users");

            Id(x => x.Username, "Username")
                .GeneratedBy.Assigned();


            //Map(x => x.Username)
            //    .WithLengthOf(50);

            Map(x => x.Password)
                .WithLengthOf(50);

            HasMany<PTIdentityRole>(x => x.RolesSet)
                .AsBag()
                .Access.AsProperty()
                .WithKeyColumn("Username");
        }
    }

    public class PTIdentityRoleMap : PTClassMap<PTIdentityRole>
    {
        public PTIdentityRoleMap()
        {
            WithTable("Roles");

            UseCompositeId()
                .WithKeyProperty(x => x.Username, "Username")
                .WithKeyProperty(x => x.Role, "Role");
        }
    }
}
