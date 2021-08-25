using Microsoft.Extensions.Configuration;
using System.Data.Common;

namespace Yerbowo.Api.Extensions
{
	public static class ConfigurationExtensions
    {
        public static string GetMySqlConnectionString(this IConfiguration configuration, string connectionString)
		{
			var db = new DbConnectionStringBuilder();
			db.ConnectionString = configuration.GetConnectionString("YerbowoDatabaseMySql");

			string host = configuration["DBHOST"] ?? db["Server"].ToString();
			string port = configuration["DBPORT"] ?? db["Port"].ToString();

			return $@"server={host};uid={db["Uid"]};pwd={db["Pwd"]};port={port};database={db["Database"]}";
		}
    }
}
