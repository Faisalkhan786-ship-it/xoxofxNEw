using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RepositoryContract;
using System.Data;
namespace Repository
{
    public class DapperContext : IDapperContext
    {
        public readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DbCon");
        }
        public IDbConnection createConnection() => new SqlConnection(_connectionString);
    }
}
