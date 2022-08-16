using Dapper;
using System.Threading.Tasks;
using static EnovaCashFlowGeneratorApi.GetWplatyWypatyCheckController;

namespace EnovaCashFlowGeneratorApi
{
    public interface IGetWplatyWyplatyCheckService
    {
        Task<string> GetWplatyWyplatyCheck(GetWplatyWypatyCheckCommand dataAccessBody, string connectionString);
    }
    public class GetWplatyWyplatyCheckService : IGetWplatyWyplatyCheckService
    {
        private readonly ICashFlowReportService _service;

        public GetWplatyWyplatyCheckService(ICashFlowReportService service)
        {
            _service = service;
        }

        public async Task<string> GetWplatyWyplatyCheck(GetWplatyWypatyCheckCommand dataAccessBody, string connectionString)
        {
            string sql = WplatyWyplatyCheckSQL.GetSql();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@StartDate", dataAccessBody.FromDate);
            parameters.Add($"@StopDate", dataAccessBody.ToDate);
            parameters.Add($"@YearToDates", dataAccessBody.YearToDate);

            return await _service.Get<WplatyWyplatyCheckModel>(sql, dataAccessBody.DbName, parameters, connectionString);
        }
    }
}