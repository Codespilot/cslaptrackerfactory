using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectTracker.Library.Data
{
    /// <summary>
    /// Specifies that the data portal
    /// should invoke a factory object rather than
    /// the business object.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DatabaseKeyAttribute : Attribute
    {
        internal static DatabaseKeyAttribute GetDatabaseKeyAttribute(Type objectType)
        {
            var result = objectType.GetCustomAttributes(typeof(DatabaseKeyAttribute), true);
            if (result != null && result.Length > 0)
                return result[0] as DatabaseKeyAttribute;
            else
                return null;
        }

        public static string GetDatabaseKeyForClass(Type objectType)
        {
            var dbInfo = DatabaseKeyAttribute.GetDatabaseKeyAttribute(objectType);
            if (dbInfo != null)
                return dbInfo.DatabaseKey;

            throw new InvalidOperationException("DatabaseKey Attribute is required on all objects using NHibernate");
        }
        /// <summary>
        /// Database Key for object, stored in App.Config
        /// </summary>
        ///
        public string DatabaseKey { get; private set; }


        /// <summary>
        /// Creates an instance of the attribute.
        /// </summary>
        /// <param name="databaseKey">
        /// Database Key
        /// </param>
        /// <remarks>
        /// The method names default to Create, Fetch,
        /// Update and Delete.
        /// </remarks>
        public DatabaseKeyAttribute(string databaseKey)
        {
            this.DatabaseKey = databaseKey;
        }


    }
}