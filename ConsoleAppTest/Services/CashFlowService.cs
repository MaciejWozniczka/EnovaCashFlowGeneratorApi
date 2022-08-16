using ConsoleAppTest.DataAccess;
using ConsoleAppTest.Models;
using EnovaCashFlowGeneratorApi;
using Newtonsoft.Json;

namespace ConsoleAppTest.Services
{
    public interface ICashFlowService
    {
        Task<CashFlowReportModel> GenerateCashFlowReport(ReportingPeriod reportingPeriod, Tenant tenant);
        Task SaveCashFlowReportToDatabase(ReportingPeriod reportingPeriod, CashFlowReportModel report);
        Task<CashFlowReportModel> GetCashFlowVersionedReport(ReportingPeriod reportingPeriod, Tenant tenant);
    }

    public class CashFlowService : ICashFlowService
    {
        private readonly IHttpDataAccess _httpDataAccess;
        CashFlowReportModel CashFlowReport;
        string url;
        string token;

        public CashFlowService(IHttpDataAccess httpDataAccess)
        {
            _httpDataAccess = httpDataAccess;
        }


        public async Task<CashFlowReportModel> GenerateCashFlowReport(ReportingPeriod reportingPeriod, Tenant tenant)
        {
            token = await GetToken();

            CashFlowReport = new CashFlowReportModel()
            {
                Tenants = await GetTenantsDbNames(token),
                RozniceKursowe = new List<RozniceKursoweModel>(),
                WplatyWyplaty = new List<WplatyWyplatyModel>(),
                WplatyWyplatyCheck = new List<WplatyWyplatyCheckModel>()
            };

            if (CashFlowReport.Tenants.Count > 0)
                await GetCashFlowReport(reportingPeriod, token);

            return CashFlowReport;
        }

        public async Task SaveCashFlowReportToDatabase(ReportingPeriod reportingPeriod, CashFlowReportModel report)
        {
            url = $"https://localhost:44379/api/Report";
            await SaveReportToDataBase(reportingPeriod, JsonConvert.SerializeObject(report.RozniceKursowe), 52);
            await SaveReportToDataBase(reportingPeriod, JsonConvert.SerializeObject(report.WplatyWyplaty), 53);
            await SaveReportToDataBase(reportingPeriod, JsonConvert.SerializeObject(report.WplatyWyplatyCheck), 54);
        }

        public async Task<CashFlowReportModel> GetCashFlowVersionedReport(ReportingPeriod reportingPeriod, Tenant tenant)
        {
            token = await GetToken();
            string roznicekursowe = (await GetVersionedReport<ReportApiExportData>(reportingPeriod, 52, token)).value[0].Data;
            string wplatywyplaty = (await GetVersionedReport<ReportApiExportData>(reportingPeriod, 53, token)).value[0].Data;
            string wplatywyplatycheck = (await GetVersionedReport<ReportApiExportData>(reportingPeriod, 54, token)).value[0].Data;

            CashFlowReport = new CashFlowReportModel()
            {
                Tenants = await GetTenantsDbNames(token),
                RozniceKursowe = JsonConvert.DeserializeObject<List<RozniceKursoweModel>>(roznicekursowe),
                WplatyWyplaty = JsonConvert.DeserializeObject<List<WplatyWyplatyModel>>(wplatywyplaty),
                WplatyWyplatyCheck = JsonConvert.DeserializeObject<List<WplatyWyplatyCheckModel>>(wplatywyplatycheck)
            };

            return CashFlowReport;
        }


        private async Task<string> GetToken()
        {
            url = "https://localhost:44379/api/Authenticate";
            CashFlowTokenDefinition loginData = new CashFlowTokenDefinition()
            {
                userName = "UserName",
                password = "Password"
            };
            return await _httpDataAccess.PostAsync<CashFlowTokenDefinition, string>(url, loginData);
        }

        private async Task<List<string>> GetTenantsDbNames(string token)
        {
            url = "https://localhost:44379/api/GetTenants";
            return await _httpDataAccess.GetAsync<List<string>>(url, token);
        }

        private async Task GetCashFlowReport(ReportingPeriod reportingPeriod, string token)
        {
            foreach (string dbName in CashFlowReport.Tenants)
            {
                await GetRozniceKursowe(dbName, reportingPeriod, token);
                await GetCashFlowWplatyWyplaty(dbName, reportingPeriod, token);
                await GetCashFlowWplatyWyplatyCheck(dbName, reportingPeriod, token);
            };
        }


        private async Task GetRozniceKursowe(string dbName, ReportingPeriod reportingPeriod, string token)
        {
            url = $"https://localhost:44379/api/RozniceKursowe?DbName={dbName}&ToDate={reportingPeriod.ToDate.ToString("yyyy-MM-dd")}";
            CashFlowReport.RozniceKursowe.AddRange(await GetFromApi<List<RozniceKursoweModel>>(url, token));
        }

        private async Task GetCashFlowWplatyWyplaty(string dbName, ReportingPeriod reportingPeriod, string token)
        {
            url = $"https://localhost:44379/api/WplatyWyplaty?DbName={dbName}&ToDate={reportingPeriod.ToDate.ToString("yyyy-MM-dd")}";
            CashFlowReport.WplatyWyplaty.AddRange(await GetFromApi<List<WplatyWyplatyModel>>(url, token));
        }

        private async Task GetCashFlowWplatyWyplatyCheck(string dbName, ReportingPeriod reportingPeriod, string token)
        {
            url = $"https://localhost:44379/api/WplatyWyplatyCheck?DbName={dbName}&FromDate={reportingPeriod.FromDate.ToString("yyyy-MM-dd")}&ToDate={reportingPeriod.ToDate.ToString("yyyy-MM-dd")}&YearToDate={reportingPeriod.YearToDate.ToString("yyyy-MM-dd")}";
            CashFlowReport.WplatyWyplatyCheck.AddRange(await GetFromApi<List<WplatyWyplatyCheckModel>>(url, token));
        }


        private async Task SaveReportToDataBase(ReportingPeriod reportingPeriod, string data, int reportId)
        {
            CashFlowDefinition body = new CashFlowDefinition()
            {
                CreateDate = reportingPeriod.CreateDate,
                TenantId = 20,
                ReportId = reportId,
                Data = JsonConvert.SerializeObject(data)
            };

            await PostToApi<CashFlowDefinition, ApiResult>(url, body);
        }

        private async Task<T> GetVersionedReport<T>(ReportingPeriod reportingPeriod, int reportId, string token)
            where T : class
        {
            url = $"https://localhost:44379/api/GetReport?createDate={reportingPeriod.CreateDate.ToString("yyyy-MM-dd.HH:mm:ss.fff")}&reportId={reportId}";
            T data = await GetVersionFromApi<T>(url, token);
            return data;
        }


        private async Task<OutputType> GetFromApi<OutputType>(string url, string token)
            where OutputType : class
        {
            try { return JsonConvert.DeserializeObject<OutputType>(await _httpDataAccess.GetAsync<string>(url, token)); }
            catch { return null; }
        }

        private async Task<OutputType> GetVersionFromApi<OutputType>(string url, string token)
            where OutputType : class
        {
            try { return await _httpDataAccess.GetAsync<OutputType>(url, token); }
            catch { return null; }
        }

        private async Task<OutputType> PostToApi<InputType, OutputType>(string url, InputType body)
            where InputType : class
            where OutputType : class
        {
            try { return await _httpDataAccess.PostAsync<InputType, OutputType>(url, body); }
            catch { return null; }
        }


        public class CashFlowTokenDefinition
        {
            public string userName { get; set; }
            public string password { get; set; }
        }
    }
}
