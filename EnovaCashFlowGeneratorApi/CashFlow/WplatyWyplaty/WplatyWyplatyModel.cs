using System;

namespace EnovaCashFlowGeneratorApi
{
    public class WplatyWyplatyModel
    {
        public string Spolka { get; set; }
        public string NumerDokumentu { get; set; }
        public DateTime Data { get; set; }
        public decimal Wplata { get; set; }
        public string WplataWaluta { get; set; }
        public decimal Wyplata { get; set; }
        public string WyplataWaluta { get; set; }
        public decimal KwotaKsiegi { get; set; }
        public string KwotaKsiegiWaluta { get; set; }
        public decimal Kwota { get; set; }
        public string KwotaWaluta { get; set; }
        public decimal DoRozliczenia { get; set; }
        public string DoRozliczeniaWaluta { get; set; }
        public string NazwaRachunku { get; set; }
        public string Kontrahent { get; set; }
        public string Opis { get; set; }
        public decimal Saldo { get; set; }
        public string Intercompany { get; set; }
        public string Stawka { get; set; }
        public string CFKod { get; set; }
        public string CF { get; set; }
        public decimal Netto { get; set; }
        public decimal VAT { get; set; }
        public string KodyISymbol { get; set; }
        public string KodyINazwa { get; set; }
        public string MpkNazwa { get; set; }
        public string SekcjaKosztowNazwa { get; set; }
        public int ZaplataID { get; set; }
        public int DokEwidencjaID { get; set; }
    }
}
