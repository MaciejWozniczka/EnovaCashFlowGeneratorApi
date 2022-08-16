using EnovaCashFlowGeneratorApi;

namespace ConsoleAppTest.Models
{
    public class CashFlowReportModel
    {
        public List<string> Tenants { get; set; }
        public List<RozniceKursoweModel> RozniceKursowe { get; set; }
        public List<WplatyWyplatyModel> WplatyWyplaty { get; set; }
        public List<WplatyWyplatyCheckModel> WplatyWyplatyCheck { get; set; }
    }
}