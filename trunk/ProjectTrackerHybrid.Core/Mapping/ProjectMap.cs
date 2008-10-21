using System.Xml;
using FluentNHibernate;
using FluentNHibernate.Mapping;
using NHibernate.Engine;
using ProjectTracker.Library.Mapping.Helpers;

namespace ProjectTracker.Library.Mapping
{
    public class ProjectMap : PTClassMap<Project>, IMapGenerator
    {
        public ProjectMap()
        {

            WithTable("Projects");

            Id(x => x.Id)
                .GeneratedBy
                .GuidComb()
                .WithUnsavedValue("00000000-0000-0000-0000-000000000000");

            Map(x => x.Name)
                .WithLengthOf(50)
                .CanNotBeNull();

            Map(x => x.Started);
            Map(x => x.Ended);
            Map(x => x.Description);

            
            HasMany<ProjectResource>(x => x.ResourcesSet)
                .AsBag()
                .Cascade.All()
                .Access.AsProperty()
                .WithKeyColumn("ProjectId")
                .WithTableName("Assignments")
                .Component(p =>
                               {
                                   p.Map(n => n.Assigned);
                                   p.Map(n => n.Role);
                                   p.References(r => r.Resource, "ResourceId");
                               });

            //Version(x => x.TimeStamp)
            //    .TheColumnNameIs("LastChanged");



            //WithTable("Assignments", m =>
            //                             {
            //                                 m.WithKeyColumn("ProjectId");
            //                                 m.Map(x => x.Id,"ProjectId");
            //                             });


            //HasManyComposite<ProjectResource>(p =>
            //                                      {
            //                                          p.Map(n => n.Assigned);
            //                                          p.Map(n => n.Role);
            //                                          p.References(r => r.Resource, "ResourceId");
            //                                      });


            //CompositeElementPart<ProjectResource> part = new CompositeElementPart<ProjectResource>();

            //part.Map(p => p.Assigned);
            //part.Map(p => p.Role);

            ////part.HasManyToOne(r => r.Resource).TheColumnNameIs("ResourceId");

            //part.Map(p => p.AssignmentId);

            //HasMany<CompositeElementPart<ProjectResource>>()

            //HasMany<ProjectResource>(x => x.ResourcesSet)
            //    .WithTableName("Assignments")
            //    .Access.AsProperty()
            //    .Cascade.All()
            //    .AsBag()
            //    .WithKeyColumn("ProjectId");

            //AddPart(part);



            //this.SetHibernateMappingAttribute("sql-insert", "exec pr_INSERTCATEGORY ?, ?");


        }

        //HasMany<Task>(x => x.ProjectTasksSet)
        //    .IsInverse()
        //    .Access.AsProperty()
        //    .AsList().WithKeyColumn("ProjectId")
        //    .Cascade.All();

        //}
    }

}
