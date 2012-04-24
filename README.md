#Needy

Helps you manage your integration tests by creating and clearing down external resources such as databases and services.

##Prerequisites

###Microsoft SQL Server - SQL Management Objects. 

If you have SQL Server installed on your local machine, you probably have the required DLLs already. However, you can get them for free as part of Microsofts own ""Microsoft SQL Server 2008 Feature Pack, August 2008".

Go here: http://www.microsoft.com/download/en/details.aspx?displaylang=en&id=16177 then search for "SharedManagementObjects.msi". Download the right installer for your machine, install and you'll be set.


##Current Usage

###Create SQL Server Database
<pre>using Needy;

[TestClass]
public void WhenNeedingACleanIntegrationDatabase
{

  [ClassInitialize]
  public static void BeforeAllTests(TestContext testContext)
 {
    Needy.Need().Database("SourceDb").As("DestDb").At("RemoteServer");
  }

  [TestInitialize]
  public void BeforeEachTest()
  {
    // Clears down (DELETES!) all data in "DestDb" on server "RemoteServer"
    Needy.Reset();
  }

  [ClassCleanUp]
  public static void AfterAllTests()
  {
    // Removes the dependency specified earlier from the global needy object
    Needy.Clear();
  }


  [TestMethod]
  public void JustUseNeedy()
  {
    // Testing with "DestDb"
    ...
  }
}
</pre>

Still work in progress, better documentation coming soon!