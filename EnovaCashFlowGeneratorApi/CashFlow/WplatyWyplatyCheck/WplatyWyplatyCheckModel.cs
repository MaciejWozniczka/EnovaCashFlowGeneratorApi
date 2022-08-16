namespace EnovaCashFlowGeneratorApi
{
    public class WplatyWyplatyCheckModel
    {
        public string Db { get; set; }
        public decimal SaldoPoprzedniegoOkresu { get; set; }
        public decimal SaldoBiezacegoOkresu { get; set; }
        public decimal RozniceKursowe { get; set; }
        public decimal PerSaldo { get; set; }
        public decimal SumaWplatWyplat { get; set; }
        public decimal Check { get; set; }
    }
}