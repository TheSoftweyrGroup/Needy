namespace Needy.Core.Dependencies
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IDependency : IDisposable
    {
        /// <summary>
        /// Run setup routine for this dependency
        /// </summary>
        void Setup();

        /// <summary>
        /// Run reset routine for this dependency
        /// </summary>
        void Reset();

        /// <summary>
        /// Is this dependency satisfied
        /// </summary>
        /// <returns>
        /// True if the dependency is usable and sufficiently enables the test to continue
        /// </returns>
        bool IsSatisfied();
    }
}
