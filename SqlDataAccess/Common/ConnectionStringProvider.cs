using System.IO;
using Microsoft.Extensions.Configuration;

namespace AnaDron.SDTest.SqlDataAccess {
	sealed class ConnectionStringProvider:IConnectionStringProvider {
		public string GetConnectionString() {
			var configurationBuilder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json");

			var configuration = configurationBuilder.Build();

			return configuration.GetConnectionString("SDTestDBConnectionString");
		}
	}
}
