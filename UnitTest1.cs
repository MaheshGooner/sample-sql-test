using SqlTestAutomation.Utils;

namespace SqlTestAutomation
{
    public class Tests
    {
        DBManager dBManager;
        [SetUp]
        public void Setup()
        {
           dBManager = new DBManager();
        }

        [Test]
        public void Test1()
        {
            string queryString = $"SELECT TOP 1 UpdatedAt,State,StateUnknown FROM [{integrationServiceDatabaseName}].[dbo].[Markets] Where EventId = {fixtureId} And marketTypeId={sportConfig.MarketTypeId} And InRunningSequenceNumber is NULL";
            dBManager.ExecuteSqlQuery(queryString);

            Assert.Pass();
        }
    }
}