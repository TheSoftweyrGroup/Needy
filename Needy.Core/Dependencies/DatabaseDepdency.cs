namespace Needy.Core.Dependencies
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    using Microsoft.SqlServer.Management.Smo;

    /// <summary>
    /// Represents a database copy command
    /// </summary>
    public class DatabaseDependency : AbstractDatabaseDependency
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseDependency"/> class.
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
        public DatabaseDependency(string serverName, string sourceDatabaseName, string destinationDatabaseName, List<string> storedProcsToCopy)
            : base(serverName, sourceDatabaseName, destinationDatabaseName, storedProcsToCopy)
        {
        }

        /// <summary>
        /// Run setup routine for this dependency
        /// </summary>
        public override void Setup()
        {
            this.CreateTables();
        }

        /// <summary>
        /// Is this dependency satisfied
        /// </summary>
        /// <returns>
        /// True if the dependency is usable and sufficiently enables the test to continue
        /// </returns>
        public override bool IsSatisfied()
        {
            var isSatisfied = this.Server.Databases.Contains(this.DestinationDatabase.Name);
            this.Server.ConnectionContext.Disconnect();
            return isSatisfied;
        }

        /// <summary>
        /// Create the database tables
        /// </summary>
        protected override void CreateTables()
        {
            try
            {
                this.Server.Databases[this.DestinationDatabase.Name].Drop();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Could not drop database '{0}'. Exception reads: {1}", this.DestinationDatabase.Name, ex.ToString()));
            }

            this.DestinationDatabase.Create();
            var transfer = new Transfer(this.SourceDatabase);
            transfer.DestinationDatabase = this.DestinationDatabase.Name;
            transfer.DestinationServer = this.Server.Name;
            transfer.CreateTargetDatabase = false;
            transfer.CopyData = false;
            transfer.CopyAllTables = true;
            transfer.Options.DriAllKeys = true;
            transfer.Options.DriForeignKeys = true;
            transfer.Options.Indexes = true;
            transfer.TransferData();
        }
    }
}
