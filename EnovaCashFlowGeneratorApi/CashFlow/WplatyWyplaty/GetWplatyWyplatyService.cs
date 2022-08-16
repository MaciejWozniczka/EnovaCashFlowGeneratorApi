using Dapper;
using System.Threading.Tasks;
using static EnovaCashFlowGeneratorApi.GetWplatyWyplatyController;

namespace EnovaCashFlowGeneratorApi
{
    public interface IGetWplatyWyplatyService
    {
        Task<string> GetWplatyWyplaty(GetWplatyWyplatyCommand dataAccessBody, string connectionString);
    }

    public class GetWplatyWyplatyService : IGetWplatyWyplatyService
    {
        private readonly ICashFlowReportService _service;

        public GetWplatyWyplatyService(ICashFlowReportService service)
        {
            _service = service;
        }

        public async Task<string> GetWplatyWyplaty(GetWplatyWyplatyCommand dataAccessBody, string connectionString)
        {
            string sql = WplatyWyplatySQL.GetSql();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@StopDate", dataAccessBody.ToDate);

            return await _service.Get<WplatyWyplatyModel>(sql, dataAccessBody.DbName, parameters, connectionString);
        }
    }
}
