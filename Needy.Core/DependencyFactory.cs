using Needy.Specifications;

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
        public static IDependency Build(DependencySpecification spec)
        {
            return spec.Build();
        }
    }
}
