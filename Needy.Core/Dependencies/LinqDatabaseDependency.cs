namespace Needy.Core.Dependencies
{
    using System.Collections.Generic;
    using System.Data.Linq;

    /// <summary>
    /// Represents a database dependency in a set of integration tests
    /// </summary>
    /// <typeparam name="T">
    /// The datacontext type that represents this dependency
    /// </typeparam>
    /// <remarks>
    /// At the time of writing, LinqToSql is out standard data access mechanism. 
    /// If this changes, or different mechanisms are required, consider refactoring this into a more flexible inheritance / interface driven implementation.
    /// </remarks>
    public class LinqDatabaseDependency<T> : AbstractDatabaseDependency where T : DataContext, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinqDatabaseDependency{T}"/> class. 
        /// </summary>
        /// <param name="serverName">
        /// The server name.
        /// </param>
        /// <param name="sourceDatabaseName">
        /// The source database name.
        /// </param>
        /// <param name="destinationDatabaseName">
        /// The destination database name.
        /// </param>
        /// <param name="storedProcsToCopy">
        /// The stored Procs To Copy.
        /// </param>
        public LinqDatabaseDependency(string serverName, string sourceDatabaseName, string destinationDatabaseName, List<string> storedProcsToCopy)
            : base(serverName, sourceDatabaseName, destinationDatabaseName, storedProcsToCopy)
        {
        }

        /// <summary>
        /// Gets or sets the name of the database to copy
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the datacontext providing this dependency
        /// </summary>
        public DataContext DataContext { get; set; }

        /// <summary>
        /// Gets or sets ServerName.
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// Run all neccessary tasks to get this database into testing order
        /// </summary>
        public override void Setup()
        {
            this.CreateTables();
            this.CreateStoredProcedures();
        }

        /// <summary>
        /// Is this dependency satisfied
        /// </summary>
        /// <returns>
        /// True if the dependency is usable and sufficiently enables the test to continue
        /// </returns>
        public override bool IsSatisfied()
        {
            return new T().DatabaseExists();
        }

        /// <summary>
        /// Creates the physical database based on the linq datacontext
        /// </summary>
        protected override void CreateTables()
        {
            var context = new T();
            if (context.DatabaseExists())
            {
                context.DeleteDatabase();
            }

            context.CreateDatabase();
        }
    }
}
