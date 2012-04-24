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
    public class DependencySpecification
    {
        /// <summary>
        /// Gets or sets DatabaseSpecification.
        /// </summary>
        public DatabaseSpecification DatabaseSpecification { get; set; }

        /// <summary>
        /// Gets or sets the source database name
        /// </summary>
        public string SourceDatabaseName { get; set; }

        /// <summary>
        /// Gets or sets LinqDataContextType.
        /// </summary>
        public Type LinqDataContextType { get; set; }

        /// <summary>
        /// Specify a database dependency
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <returns>
        /// A new database dependency
        /// </returns>
        public DatabaseSpecification Database(string source)
        {
            this.DatabaseSpecification = new DatabaseSpecification();
            this.SourceDatabaseName = source;
            return this.DatabaseSpecification;
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
        public DatabaseSpecification Database<T>(string source) where T : DataContext
        {
            this.SourceDatabaseName = source;
            this.LinqDataContextType = typeof(T);
            return new DatabaseSpecification();
        }
    }
}
