namespace Needy
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;

    using global::Needy.Core;
    using global::Needy.Core.Dependencies;
    using global::Needy.Specifications;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Needy
    {
        /// <summary>
        /// Collection of dependencies
        /// </summary>
        private static readonly List<DependencySpecification> specs = new List<DependencySpecification>();

        /// <summary>
        /// Collection of built dependencies
        /// </summary>
        private static readonly List<IDependency> registeredDependencies = new List<IDependency>();

        /// <summary>
        /// Dependencies that couldn't be built
        /// </summary>
        private static readonly List<DependencySpecification> dissatisfiedSpecs = new List<DependencySpecification>();

        /// <summary>
        /// Gets Specs.
        /// </summary>
        public static List<DependencySpecification> Specs
        {
            get
            {
                return specs;
            }
        }

        /// <summary>
        /// Gets dependencies
        /// </summary>
        public static List<IDependency> RegisteredDependencies
        {
            get
            {
                return registeredDependencies;
            }
        }

        /// <summary>
        /// Gets DissatisfiedSpecs.
        /// </summary>
        public static List<DependencySpecification> DissatisfiedSpecs
        {
            get
            {
                return dissatisfiedSpecs;
            }
        }

        /// <summary>
        /// Specify a dependency
        /// </summary>
        /// <returns>
        /// Returns a DependencySpecification instance
        /// </returns>
        public static DependencySpecification Need()
        {
            var spec = new DependencySpecification();
            specs.Add(spec);
            return spec;
        }

        /// <summary>
        /// Create registered dependencies
        /// </summary>
        public static void Create()
        {
            foreach (var spec in specs)
            {
                var dependency = DependendencyFactory.Build(spec);
                dependency.Setup();
                if (!dependency.IsSatisfied())
                {
                    dissatisfiedSpecs.Add(spec);
                }
            }
        }

        /// <summary>
        /// Invoke reset functionality of all registered dependencies
        /// </summary>
        public static void Reset()
        {
            foreach (var dependency in registeredDependencies)
            {
                dependency.Reset();
            }
        }

        /// <summary>
        /// Remove existing dependencies
        /// </summary>
        public static void Clear()
        {
            specs.Clear();
        }
    }
}