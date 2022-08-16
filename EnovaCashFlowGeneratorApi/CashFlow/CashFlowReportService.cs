using Dapper;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace EnovaCashFlowGeneratorApi
{
    public interface ICashFlowReportService
    {
        Task<string> Get<T>(string sql, string dbName, DynamicParameters parameters, string connectionString) where T : class;
    }

    public class CashFlowReportService : ICashFlowReportService
    {
        private readonly IEnovaDataAccess _enovaDataAccess;

        public CashFlowReportService(IEnovaDataAccess enovaDataAccess)
        {
            _enovaDataAccess = enovaDataAccess;
        }

        public async Task<string> Get<T>(string sql, string dbName, DynamicParameters parameters, string connectionString) where T : class
        {
            var output = await _enovaDataAccess.GetAsync<T>(sql, dbName, connectionString, parameters);

            return JsonConvert.SerializeObject(output);
        }
    }
}