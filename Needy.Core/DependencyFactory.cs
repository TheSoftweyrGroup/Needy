namespace Needy.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Dependencies;

    /// <summary>
    /// Functionality for getting an instance of a depdendency
    /// </summary>
    public class DependendencyFactory
    {
        /// <summary>
        /// Get an a concrete implementation of IDependency based on details in spec
        /// </summary>
        /// <param name="spec">
        /// The spec.
        /// </param>
        /// <returns>
        /// A concrete type adhering to IDependency
        /// </returns>
        public static IDependency Build(Specifications.DependencySpecification spec)
        {
            // By default, the local machine
            string server = ".";
            if (spec.DatabaseSpecification.LocationSpecification != null)
            {
                server = spec.DatabaseSpecification.LocationSpecification.Location;
            }

            var source = spec.SourceDatabaseName;
            var destination = spec.DatabaseSpecification.DestinationName;

            if (spec.LinqDataContextType != null)
            {
                var dependency = typeof(LinqDatabaseDependency<>).MakeGenericType(spec.LinqDataContextType);
                return (IDependency)Activator.CreateInstance(dependency, server, source, destination, new List<string>());
            }
            else
            {
                return new DatabaseDependency(server, source, destination, new List<string>());
            }
        }
    }
}
