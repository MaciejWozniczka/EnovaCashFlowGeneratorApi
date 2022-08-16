using Dapper.Contrib.Extensions;

namespace ConsoleAppTest.Models
{
    [Table("ReportData")]
    public class ReportExportData : ABaseEntity
    {
        public DateTime CreateDate { get; set; }
        public int ReportId { get; set; }

        [Write(false)]
        public string ReportName { get; set; }
        public string Data { get; set; }
    }

    public class ReportApiExportData
    {
        public List<ReportExportData> value { get; set; }
        public bool success { get; set; }
        public object errors { get; set; }
        public int code { get; set; }
    }
}