using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlTestAutomation.Config
{
    public static class ConfigurationFactory
    {

        private static IConfigurationRoot configuration;
        private static readonly object LockObject = new();
        private const string EnvironmentVariableName = "ASPNETCORE_ENVIRONMENT";

        public static IConfiguration GetConfiguration()
        {
            lock (LockObject)
            {
                if (configuration == null)
                {
                    var t = Environment.GetEnvironmentVariables();
                    var environment = Environment.GetEnvironmentVariable(EnvironmentVariableName);
                    if (string.IsNullOrWhiteSpace(environment)) throw new InvalidOperationException($"{nameof(EnvironmentVariableName)} is not setup");

                    configuration = new ConfigurationBuilder()
                        //.AddJsonFile($"appsettings.{environment.ToLower()}.json", true)
                        .AddJsonFile($"dbsettings.json", true)
                        .Build();
                }
            }

            return configuration;
        }
    }
}
