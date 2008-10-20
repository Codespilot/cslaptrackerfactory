using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectTracker.Library.Mapping
{
    public class ProjectInfoMap : PTClassMap<ProjectInfo>
    {
        public ProjectInfoMap()
        {
            WithTable("Projects");

            Id(x => x.Id);

            Map(x => x.Name);

        }
    }
}
