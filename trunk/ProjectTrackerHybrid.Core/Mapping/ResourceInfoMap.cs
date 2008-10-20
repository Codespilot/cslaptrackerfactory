using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectTracker.Library.Mapping.Helpers;

namespace ProjectTracker.Library.Mapping
{
    public class ResourceInfoMap : PTClassMap<ResourceInfo>, IMapGenerator
    {
        public ResourceInfoMap()
        {
            WithTable("Resources");

            Id(x => x.Id);

            Map(x => x.FirstName);
            Map(x => x.LastName);
        }
    }
}
