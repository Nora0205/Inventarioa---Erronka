using System;

namespace StockBaseApp.Modeloak
{
    public class Inprimagailua(int id, string marka, string kokalekua, string egoera, DateTime erosketaData, bool koloretakoa) 
        : Gailua(id, marka, kokalekua, egoera, erosketaData)
    {
        public bool Koloretakoa { get; set; } = koloretakoa;

        public override string LortuInformazioa()
        {
            string colorTxt = Koloretakoa ? "Bai" : "Ez";
            return base.LortuInformazioa() + $" | Koloretakoa: {colorTxt}";
        }
    }
}