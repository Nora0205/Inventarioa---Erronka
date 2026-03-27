using System;

namespace StockBaseApp.Modeloak
{
    public class Inprimagailua : Gailua
    {
        public bool Koloretakoa { get; set; }

        public Inprimagailua(int id, string marka, string kokalekua, string egoera, DateTime erosketaData, bool koloretakoa)
            : base(id, marka, kokalekua, egoera, erosketaData)
        {
            Koloretakoa = koloretakoa;
        }

        public override string LortuInformazioa()
        {
            string colorTxt = Koloretakoa ? "Bai" : "Ez";
            return base.LortuInformazioa() + $" | Koloretakoa: {colorTxt}";
        }
    }
}