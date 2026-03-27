using System;

namespace StockBaseApp.Modeloak
{
    public class Gailua
    {
        public int IdGailua { get; set; }
        public string Marka { get; set; } = "";
        public string Kokalekua { get; set; } = "";
        public string Egoera { get; set; } = "";
        public DateTime ErosketaData { get; set; }

        public Gailua(int id, string marka, string kokalekua, string egoera, DateTime erosketaData)
        {
            IdGailua = id;
            Marka = marka;
            Kokalekua = kokalekua;
            Egoera = egoera;
            ErosketaData = erosketaData;
        }

        public virtual string LortuInformazioa()
        {
            return $"ID: {IdGailua} | Marka: {Marka} | Kokalekua: {Kokalekua}";
        }
    }
}