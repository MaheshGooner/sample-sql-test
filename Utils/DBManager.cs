using Microsoft.Extensions.Configuration;
using SqlTestAutomation.Config;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlTestAutomation.Utils
{
    public class DBManager
    {
        IConfiguration configuration = ConfigurationFactory.GetConfiguration();

        public void ExecuteSqlQuery(string query)
        {
            //var sportConfig = GetSportConfig();
            //var tfuPublishedTime = SecretsConfiguration.getSecret("TFU_PublishedAt");
            
            string connectionString = $"{configuration.GetSection("IntegrationService:Server").Value};Database={integrationServiceDatabaseName};User Id={integrationServiceUsername};Password={SecretsConfiguration.getSecret($"IS_PASSWORD_{SecretsConfiguration.getEnvironment()}")};";

            using SqlConnection connection = new(connectionString);
            SqlCommand command = new(queryString, connection);
            //Retry.Do(() => connection.Open(), TimeSpan.FromSeconds(10), 12);
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                Assert.IsTrue(reader.HasRows, $"Market with MarketType {sportConfig.MarketTypeId} is not available in IS database for fixture {fixtureId}");
                while (reader.Read())
                {
                    Console.WriteLine($"MarketTypeId {sportConfig.MarketTypeId} was last updated at: {reader["UpdatedAt"]} for fixture {fixtureId}");

                    Assert.IsTrue(reader["State"].Equals(4), $"Market State for MarketType {sportConfig.MarketTypeId} is {reader["State"]}, but is expected to be 4 (Resulted) in IS database for fixture {fixtureId}");
                    Console.WriteLine($"MarketTypeId {sportConfig.MarketTypeId} has state: {reader["State"]} for fixture {fixtureId}");

                    Assert.IsTrue(reader["StateUnknown"].Equals(false), $"Market for MarketType {sportConfig.MarketTypeId} has StateUnknown set to True (1) in IS database for fixture {fixtureId}");
                    Console.WriteLine($"MarketTypeId {sportConfig.MarketTypeId} has StateUnknown value set to: {reader["StateUnknown"]} for fixture {fixtureId}");

                    if (tfuPublishedTime != null)
                    {
                        Console.WriteLine($"Time taken since the initial spoofed TradingFeedUpdateDto from market tracker to IS service receiving a price UpdatedAt is: {DateTimeOffset.Parse(reader["UpdatedAt"].ToString()).UtcDateTime - DateTimeOffset.Parse(tfuPublishedTime).UtcDateTime}(hh:mm:ss) for fixture {fixtureId}");
                    }
                }
            }
            finally
            {
                reader.Close();
            }

        }

        public string GetConnectionString()
        {
            string connectionString = $"{configuration.GetSection("dbconnection:dbinfo").Value};Database={integrationServiceDatabaseName};User Id={integrationServiceUsername};Password={SecretsConfiguration.getSecret($"IS_PASSWORD_{SecretsConfiguration.getEnvironment()}")};";


            return "";
        }
    }
}
