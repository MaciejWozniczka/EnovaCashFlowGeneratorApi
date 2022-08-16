using Dapper.Contrib.Extensions;

namespace ConsoleAppTest.Models
{
    public class ReportingPeriod : ABaseEntity
    {
        public DateTime CreateDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string VersionName { get; set; }
        public bool IsNew { get; set; }
        public int TenantId { get; set; }

        [Write(false)]
        public DateTime YearToDate => FromDate.AddDays(-1);

        public List<Tuple<DateTime, DateTime>> Months = new List<Tuple<DateTime, DateTime>>();
    }
}
