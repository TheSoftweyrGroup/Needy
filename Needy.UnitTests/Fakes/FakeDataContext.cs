namespace Needy.UnitTests.Fakes
{
    using System;
    using System.Collections.Generic;
    using System.Data.Linq;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FakeDataContext : DataContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FakeDataContext"/> class.
        /// </summary>
        public FakeDataContext()
            : base("FakeConnectionString")
        { 
        }
    }
}
