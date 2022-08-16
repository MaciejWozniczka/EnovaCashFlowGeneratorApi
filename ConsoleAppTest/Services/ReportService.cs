using ConsoleAppTest.DataAccess;
using ConsoleAppTest.Models;
using Dapper;
using EnovaCashFlowGeneratorApi;
using Newtonsoft.Json;

namespace ConsoleAppTest.Services
{
    public class ReportService
    {
        private readonly ICashFlowService _cashFlowReportService;
        private readonly ISqlDataAccess _dataAccess;

        public ReportService(ICashFlowService cashFlowReportService)
        {
            _cashFlowReportService = cashFlowReportService;
        }

        public async Task<CashFlowReportModel> GetCashFlowReport(Tenant tenant, ReportingPeriod reportingPeriod)
        {
            var report = await GetCashFlowVersionedReport(reportingPeriod, tenant);

            if (reportingPeriod.IsNew && report.RozniceKursowe == null && report.WplatyWyplaty == null && report.WplatyWyplatyCheck == null)
                report = await GenerateCashFlowReport(reportingPeriod, tenant);

            return report;
        }

        private async Task<CashFlowReportModel> GenerateCashFlowReport(ReportingPeriod reportingPeriod, Tenant tenant)
        {
            CashFlowReportModel output = await _cashFlowReportService.GenerateCashFlowReport(reportingPeriod, tenant);

            _dataAccess.Add(new ReportExportData() { CreateDate = reportingPeriod.CreateDate, ReportId = 52, Data = System.Text.Json.JsonSerializer.Serialize(output.RozniceKursowe) });
            _dataAccess.Add(new ReportExportData() { CreateDate = reportingPeriod.CreateDate, ReportId = 53, Data = System.Text.Json.JsonSerializer.Serialize(output.WplatyWyplaty) });
            _dataAccess.Add(new ReportExportData() { CreateDate = reportingPeriod.CreateDate, ReportId = 54, Data = System.Text.Json.JsonSerializer.Serialize(output.WplatyWyplatyCheck) });

            return output;
        }

        private async Task<CashFlowReportModel> GetCashFlowVersionedReport(ReportingPeriod reportingPeriod, Tenant tenant)
        {
            try
            {
                return new CashFlowReportModel()
                {
                    RozniceKursowe = JsonConvert.DeserializeObject<List<RozniceKursoweModel>>((await GetFlatReport(52, reportingPeriod)).Data),
                    WplatyWyplaty = JsonConvert.DeserializeObject<List<WplatyWyplatyModel>>((await GetFlatReport(53, reportingPeriod)).Data),
                    WplatyWyplatyCheck = JsonConvert.DeserializeObject<List<WplatyWyplatyCheckModel>>((await GetFlatReport(54, reportingPeriod)).Data)
                };
            }
            catch
            {
                return new CashFlowReportModel();
            }
        }

        public async Task<ReportExportData> GetFlatReport(int reportId, ReportingPeriod reportingPeriod)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ReportId", reportId);
            parameters.Add("@CreateDate", reportingPeriod.CreateDate);

            string sql = $@"
SELECT *
FROM [dbo].[ReportData]
WHERE ReportId = @ReportId
AND CreateDate = @CreateDate
";

            var output = await _dataAccess.GetAsync<ReportExportData>(sql, parameters);

            return output.FirstOrDefault();
        }
    }
}