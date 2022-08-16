using Dapper;
using System.Threading.Tasks;
using static EnovaCashFlowGeneratorApi.GetRozniceKursoweController;

namespace EnovaCashFlowGeneratorApi
{
    public interface IGetRozniceKursoweService
    {
        Task<string> GetRozniceKursowe(GetRozniceKursoweCommand dataAccessBody, string connectionString);
    }
    public class GetRozniceKursoweService : IGetRozniceKursoweService
    {
        private readonly ICashFlowReportService _service;

        public GetRozniceKursoweService(ICashFlowReportService service)
        {
            _service = service;
        }

        public async Task<string> GetRozniceKursowe(GetRozniceKursoweCommand dataAccessBody, string connectionString)
        {
            string sql = RozniceKursoweSQL.GetSql();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@StopDate", dataAccessBody.ToDate);

            return await _service.Get<RozniceKursoweModel>(sql, dataAccessBody.DbName, parameters, connectionString);
        }
    }
}
