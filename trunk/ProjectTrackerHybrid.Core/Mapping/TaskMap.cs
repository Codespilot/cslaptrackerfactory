using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Mapping;

namespace ProjectTracker.Library.Mapping
{
    public class TaskMap : ClassMap<Task>
    {
        public TaskMap()
        {
            WithTable("ProjectTask");

            Id(x => x.Id).GeneratedBy.GuidComb()
                .WithUnsavedValue("00000000-0000-0000-0000-000000000000");

            
            
            
            Map(x => x.ProjectId).CanNotBeNull();

            Map(x => x.Name)
                .WithLengthOf(50).CanNotBeNull();

            Map(x => x.Description)
                .WithLengthOf(100).CanNotBeNull();
        }
    }
}
