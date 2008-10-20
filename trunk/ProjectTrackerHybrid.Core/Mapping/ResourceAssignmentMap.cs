using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectTracker.Library.Mapping.Helpers;

namespace ProjectTracker.Library.Mapping
{
    public class ResourceAssignmentMap : PTClassMap<ResourceAssignment>, IMapGenerator
    {
        public ResourceAssignmentMap()
        {
            WithTable("Assignments");

            UseCompositeId()
                .WithKeyProperty(r => r.ResourceId, "ResourceId")
                .WithKeyProperty(x => x.ProjectId, "ProjectId");

            //Map(x => x.ProjectId);
            //Map(x => x.ResourceId);

            Map(x => x.Assigned);
            Map(x => x.Role);



            References(x => x.Project).TheColumnNameIs("ProjectId");//.SetAttribute("update", "false");

            Version(x => x.TimeStamp)
                .TheColumnNameIs("LastChanged");

        }
    }
}
