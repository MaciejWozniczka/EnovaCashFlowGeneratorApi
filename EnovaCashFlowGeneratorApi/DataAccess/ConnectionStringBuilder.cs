using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace EnovaCashFlowGeneratorApi
{
    public interface IConnectionStringBuilder
    {
        string BuildConnectionEnova(string tenantIdentifier);
        string BuildConnectionSqlDb();
    }

    public class ConnectionStringBuilder : IConnectionStringBuilder
    {
        private readonly IConfiguration _configuration;

        public ConnectionStringBuilder(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string BuildConnectionEnova(string tenantIdentifier)
        {
            var defaultConnection = _configuration.GetConnectionString("EnovaDb");
            var builder = new SqlConnectionStringBuilder(defaultConnection);
            builder.InitialCatalog = tenantIdentifier;
            return builder.ConnectionString;
        }
        public string BuildConnectionSqlDb()
        {
            var defaultConnection = _configuration.GetConnectionString("SqlDb");
            var builder = new SqlConnectionStringBuilder(defaultConnection);
            return builder.ConnectionString;
        }
    }
}