using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using FluentNHibernate;
using FluentNHibernate.Mapping;
using ProjectTracker.Library.Mapping.Helpers;

namespace ProjectTracker.Library.Mapping
{
    public class ProjectResourceMap : PTClassMap<ProjectResource>, IMapGenerator
    {
        public ProjectResourceMap()
        {
            WithTable("Assignments");

            UseCompositeId()
                .WithKeyProperty(r => r.ProjectId, "ProjectId")
                .WithKeyReference(x => x.Resource, "ResourceId");

            //Map(x => x.ProjectId);
            //Map(x => x.ResourceId);
            Map(x => x.Assigned);
            Map(x => x.Role);

            References(x => x.Resource).TheColumnNameIs("ResourceId"); //.SetAttribute("update", "false"); ;

            Version(x => x.TimeStamp)
                .TheColumnNameIs("LastChanged");


        }
    }

}
