using Needy.Core.Dependencies;

namespace Needy.Specifications
{
    using System;
    using System.Collections.Generic;
    using System.Data.Linq;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Dependency Specifications
    /// </summary>
    public class DependencySpecification : IDependencySpecificationRoot
    {
        private IDependencySpecification firstSpecification; 

        IDependencySpecification IDependencySpecificationRoot.Specification { set { firstSpecification = value; } }

        internal IDependency Build()
        {
            return this.firstSpecification.Build();
        }
    }

    public interface IDependencySpecificationRoot
    {
        IDependencySpecification Specification { set; }
    }

    public interface IDependencySpecification
    {
        IDependency Build();
    }

    public class SourceDatabaseSpecification : IDependencySpecification
    {
        /// <summary>
        /// Initializes a new instance of the SourceDatabaseSpecification class.
        /// </summary>
        /// <param name="source">
        /// The source database name
        /// </param>
        /// <param name="LinqDataContextType">
        /// LinqDataContextType.
        /// </param>
        public SourceDatabaseSpecification(string source, Type linqDataContextType, DestinationDatabaseSpecification destinationDatabaseSpecification)
        {
            this.SourceDatabaseName = source;
            this.LinqDataContextType = linqDataContextType;
            this.DestinationDatabaseSpecification = destinationDatabaseSpecification;
        }

        /// <summary>
        /// Gets or sets the source database name
        /// </summary>
        public string SourceDatabaseName { get; private set; }

        /// <summary>
        /// Gets or sets LinqDataContextType.
        /// </summary>
        public Type LinqDataContextType { get; private set; }

        /// <summary>
        /// Gets or sets DatabaseSpecification.
        /// </summary>
        public DestinationDatabaseSpecification DestinationDatabaseSpecification { get; set; }

        IDependency IDependencySpecification.Build()
        {
            // By default, the local machine
            string server = ".";
            if (this.DestinationDatabaseSpecification.LocationSpecification != null)
            {
                server = this.DestinationDatabaseSpecification.LocationSpecification.Location;
            }

            var source = this.SourceDatabaseName;
            var destination = this.DestinationDatabaseSpecification.DestinationName;

            if (this.LinqDataContextType != null)
            {
                var dependency = typeof(LinqDatabaseDependency<>).MakeGenericType(this.LinqDataContextType);
                return (IDependency)Activator.CreateInstance(dependency, server, source, destination, new List<string>());
            }
            else
            {
                return new DatabaseDependency(server, source, destination, new List<string>());
            }
        }
    }

    public static class DependencySpecificationDatabaseExtensions
    {
        /// <summary>
        /// Specify a database dependency
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <returns>
        /// A new database dependency
        /// </returns>
        public static DestinationDatabaseSpecification Database(this DependencySpecification owner, string source)
        {
            var destinationSpecification = new DestinationDatabaseSpecification();
            var sourceSpecification = new SourceDatabaseSpecification(source, null, destinationSpecification);
            ((IDependencySpecificationRoot)owner).Specification = sourceSpecification;
            return destinationSpecification;
        }

        /// <summary>
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// </returns>
        public static DestinationDatabaseSpecification Database<T>(this DependencySpecification owner, string source) where T : DataContext
        {
            var destinationSpecification = new DestinationDatabaseSpecification();
            var sourceSpecification = new SourceDatabaseSpecification(source, typeof(T), destinationSpecification);
            ((IDependencySpecificationRoot)owner).Specification = sourceSpecification;
            return destinationSpecification;
        }
    }
}
