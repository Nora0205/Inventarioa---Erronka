using System;

namespace StockBaseApp.Modeloak
{
    public class Gailua(int id, string marka, string kokalekua, string egoera, DateTime erosketaData, int idMintegia = 1, string modeloa = "")
    {
        public int IdGailua { get; set; } = id;
        public string Marka { get; set; } = marka;
        public string Modeloa { get; set; } = modeloa;
        public string Kokalekua { get; set; } = kokalekua;
        public string Egoera { get; set; } = egoera;
        public DateTime ErosketaData { get; set; } = erosketaData;
        public int IdMintegia { get; set; } = idMintegia;

        public virtual string LortuInformazioa()
        {
            return $"ID: {IdGailua} | Marka: {Marka} | Kokalekua: {Kokalekua}";
        }
    }
}