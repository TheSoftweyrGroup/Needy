namespace Needy.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using global::Needy;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class WhenNeedingADependency
    {
        [TestCleanup]
        public void AfterEachTest()
        {
            Needy.Clear();
        }

        [TestMethod]
        public void GivenStandardDatabaseDependencyAllShouldBeRegistered()
        {
            // Arrange
            var expectedDependencies = 2;

            // Act
            Needy.Need().Database("SourceDb").As("DestDb").At("RemoteServer");
            Needy.Need().Database("AnotherSourceDb").As("AnotherDestDb");

            // Assert
            Assert.AreEqual(expectedDependencies, Needy.Specs.Count);
            var firstDbSpec = Needy.Specs.First();
            Assert.AreEqual("SourceDb", firstDbSpec.SourceDatabaseName);
        }

        [TestMethod]
        public void LinqDependencyShouldBeRegisterd()
        {
            // Arrange
            var expectedDependencies = 1;

            // Act
            Needy.Need().Database<Fakes.FakeDataContext>("SourceDb").As("DestDb").At("RemoteServer");

            // Assert
            Assert.AreEqual(expectedDependencies, Needy.Specs.Count);
        }

    }
}
