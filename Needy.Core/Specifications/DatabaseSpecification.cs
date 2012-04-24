namespace Needy.Specifications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents a database dependency
    /// </summary>
    public class DatabaseSpecification
    {
        /// <summary>
        /// Gets or sets LocationSpecification.
        /// </summary>
        public LocationSpecification LocationSpecification { get; set; }

        /// <summary>
        /// Gets or sets DestinationName.
        /// </summary>
        public string DestinationName { get; set; }

        /// <summary>
        /// Specify the destination of the database dependency
        /// </summary>
        /// <param name="destinationName">
        /// The destination name.
        /// </param>
        /// <returns>
        /// The location specification
        /// </returns>
        public LocationSpecification As(string destinationName)
        {
            this.LocationSpecification = new LocationSpecification();
            this.DestinationName = destinationName;
            return this.LocationSpecification;
        }
    }
}
