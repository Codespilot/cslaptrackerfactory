using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Xml;
using FluentNHibernate;
using FluentNHibernate.Mapping;
using ProjectTracker.Library.Mapping.Helpers;

namespace ProjectTracker.Library.Mapping
{
    public abstract class PTClassMap<T> : ClassMap<T>
    {
        /// <summary>
        /// This overloaded version is required so that we can use optimistic concurrency with a sql timestamp in the database.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public override VersionPart Version(Expression<System.Func<T, object>> expression)
        {
            
            var versionPart = new VersionPart(ReflectionHelper.GetProperty(expression));

            versionPart.SetAttribute("type", "ProjectTracker.Library.Mapping.UserTypeTimestamp, ProjectTracker.Library");
            versionPart.SetAttribute("generated", "always");
            versionPart.SetAttribute("unsaved-value", "null");

            AddPart(versionPart);

            return versionPart;

        }

        ////TODO:MAKE A TEST FOR THIS
        //public virtual ManyToOnePart HasManyToOne(Expression<Func<T, object>> expression)
        //{
        //    PropertyInfo property = ReflectionHelper.GetProperty(expression);
        //    ManyToOnePart part = new ManyToOnePart(property);

        //    AddPart(part);
        //    return part;
        //}


        public XmlDocument Generate()
        {
            return CreateMapping(new MappingVisitor());
        }
    }
}
