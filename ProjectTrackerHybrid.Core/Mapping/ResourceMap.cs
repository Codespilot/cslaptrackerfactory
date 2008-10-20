using FluentNHibernate.Mapping;
using ProjectTracker.Library.Mapping.Helpers;

namespace ProjectTracker.Library.Mapping
{

    public class ResourceMap : PTClassMap<Resource>, IMapGenerator
    {
        public ResourceMap()
        {
            WithTable("Resources");

            Id(x => x.Id);

            Map(x => x.FirstName)
                .WithLengthOf(50);
            Map(x => x.LastName)
                .WithLengthOf(50);

            //CompositeElementPart<ResourceAssignment> part = new CompositeElementPart<ResourceAssignment>();

            //part.Map(p => p.Assigned);
            //part.Map(p => p.Role);

            //part.HasManyToOne(p => p.Project).TheColumnNameIs("ProjectId");

            //AddPart(part);

            HasMany<ResourceAssignment>(x => x.AssignmentsSet)
                .WithTableName("Assignments")
                .Access.AsProperty()
                .Cascade.All()
                .AsBag()
                .WithKeyColumn("ResourceId")
                .Component(p =>
                               {
                                   p.Map(n => n.Role);
                                   p.Map(n => n.Assigned);
                                   p.References(r => r.Project, "ProjectId");
                               });

        }
    }
}
