using Dapper.Contrib.Extensions;

namespace ConsoleAppTest.Models
{
    public class Tenant : ABaseEntity
    {
        public string Name { get; set; }
        public string DbName { get; set; }
        public string DbType { get; set; }
        public string City { get; set; }
        public string Department { get; set; }
        public string Intercompany { get; set; }
        public string IntercompanyTax { get; set; }
        public DateTime AccountingYearStartDate { get; set; }
        public DateTime AccountingYearEndDate { get; set; }

        [Write(false)]
        public TenantDetails Details { get; set; }
    }

    [Table("TenantDetails")]
    public class TenantDetails : ABaseEntity
    {
        public int TenantId { get; set; }
        public string TenantFullName { get; set; }
        public string TenantShortName { get; set; }
        public string NIP { get; set; }
        public string REGON { get; set; }
        public string Address { get; set; }
        public string IRSName { get; set; }
        public string IRSAddress { get; set; }
    }

    [Table("TenantsAccountingYears")]
    public class TenantsAccountingYears : ABaseEntity
    {
        public int TenantId { get; set; }
        public DateTime AccountingYearStartDate { get; set; }
        public DateTime AccountingYearEndDate { get; set; }
    }
}