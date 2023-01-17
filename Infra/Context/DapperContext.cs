using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Infra.Context
{
	public class DapperContext : IDisposable
	{
		private readonly IConfiguration _config;
		private readonly string _connectionString;

		public DapperContext(IConfiguration config)
		{
			_config = config;
			_connectionString = _config.GetConnectionString("connStr");
		}

		public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);

		public void Dispose()
		{
			CreateConnection().Close();
		}
	}
}