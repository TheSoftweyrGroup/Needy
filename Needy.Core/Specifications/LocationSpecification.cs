namespace Needy.Specifications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Location Specification
    /// </summary>
    public class LocationSpecification
    {
        /// <summary>
        /// Gets or sets Location.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Specify a location
        /// </summary>
        /// <param name="location">
        /// The location.
        /// </param>
        public void At(string location)
        {
            this.Location = location;
        }
    }
}
