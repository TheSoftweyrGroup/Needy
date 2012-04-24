namespace Needy.Core.Dependencies
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Microsoft.SqlServer.Management.Common;
    using Microsoft.SqlServer.Management.Smo;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class AbstractDatabaseDependency : IDependency
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractDatabaseDependency"/> class.
        /// </summary>
        protected AbstractDatabaseDependency()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractDatabaseDependency"/> class. 
        /// </summary>
        /// <param name="server">
        /// The server.
        /// </param>
        /// <param name="databaseName">
        /// The database name.
        /// </param>
        /// <param name="destinationDatabaseName">
        /// The destination Database Name.
        /// </param>
        /// <param name="storedProcsToCopy">
        /// The stored Procs To Copy.
        /// </param>
        protected AbstractDatabaseDependency(string server, string databaseName, string destinationDatabaseName, List<string> storedProcsToCopy)
        {
            this.Server = new Server(server);
            this.SourceDatabase = this.Server.Databases[databaseName];
            this.DestinationDatabase = new Database(this.Server, destinationDatabaseName);
            this.StoredProcsToCopy = storedProcsToCopy;
        }

        /// <summary>
        /// Gets or sets Server.
        /// </summary>
        protected Server Server { get; set; }

        /// <summary>
        /// Gets or sets SourceDatabase.
        /// </summary>
        protected Database SourceDatabase { get; set; }

        /// <summary>
        /// Gets or sets DestinationDatabase.
        /// </summary>
        protected Database DestinationDatabase { get; set; }

        /// <summary>
        /// Gets or sets StoredProcsToCopy.
        /// </summary>
        protected List<string> StoredProcsToCopy { get; set; }

        /// <summary>
        /// Run setup routine for this dependency
        /// </summary>
        public abstract void Setup();

        /// <summary>
        /// Run reset routine for this dependency
        /// </summary>
        /// <summary>
        /// Run reset routine for this dependency
        /// </summary>
        public void Reset()
        {
            var deleteFormat = "DELETE FROM {0};";
            var deleteStatment = new StringBuilder();

            // Delete data in all tables
            this.DestinationDatabase.Refresh();
            foreach (Table table in this.DestinationDatabase.Tables)
            {
                deleteStatment.AppendLine(string.Format(deleteFormat, table.Name));
                foreach (ForeignKey fk in table.ForeignKeys)
                {
                    fk.IsEnabled = false;
                }

                table.Alter();
            }

            // Delete all data
            this.DestinationDatabase.ExecuteNonQuery(deleteStatment.ToString());

            // Re-enable Foriegn keys
            foreach (Table table in this.DestinationDatabase.Tables)
            {
                deleteStatment.AppendLine(string.Format(deleteFormat, table.Name));
                foreach (ForeignKey fk in table.ForeignKeys)
                {
                    fk.IsEnabled = true;
                }

                table.Alter();
            }
        }

        /// <summary>
        /// Is this dependency satisfied
        /// </summary>
        /// <returns>
        /// True if the dependency is usable and sufficiently enables the test to continue
        /// </returns>
        public abstract bool IsSatisfied();

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            this.DestinationDatabase.Drop();
            this.Server = null;
            this.SourceDatabase = null;
            this.DestinationDatabase = null;
        }

        /// <summary>
        /// Create tables in destination database
        /// </summary>
        protected abstract void CreateTables();

        private void CopyStoredProc(StoredProcedure sp, Database db)
        {
            if (!sp.IsSystemObject)
            {
                try
                {
                    this.DestinationDatabase.ExecuteNonQuery(sp.Script());
                }
                catch (Exception ex)
                {
                    var errorMessage = string.Format(
                        "Failed to create proc '{0}'. Exception reads: '{1}{2}'", sp.Name, ex.Message, ex.StackTrace);
                    Console.WriteLine(errorMessage);
                }
            }
        }

        /// <summary>
        /// Create stored procedures in the destination database
        /// </summary>
        protected void CreateStoredProcedures()
        {
            if (this.StoredProcsToCopy.Count < 1)
            {
                foreach (StoredProcedure sp in this.SourceDatabase.StoredProcedures)
                {
                    this.CopyStoredProc(sp, this.DestinationDatabase);
                }
            }
            else
            {
                foreach (var procName in this.StoredProcsToCopy)
                {
                    try
                    {
                        var sp = this.SourceDatabase.StoredProcedures[procName];
                        this.CopyStoredProc(sp, this.DestinationDatabase);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + ex.StackTrace);
                    }
                }
            }
        }
    }
}
