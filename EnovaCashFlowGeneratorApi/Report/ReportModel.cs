using System;

namespace EnovaCashFlowGeneratorApi
{
    public class ReportModel : BaseModel
    {
        public int TenantId { get; set; }
        public int ReportId { get; set; }
        public string Data { get; set; }
    }
}
