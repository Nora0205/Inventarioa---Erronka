using System;

namespace StockBaseApp.Modeloak
{
    public class Gailua
    {
        public int IdGailua { get; set; }
        public string Marka { get; set; } = "";
        public string Modeloa { get; set; } = "";
        public string Kokalekua { get; set; } = "";
        public string Egoera { get; set; } = "";
        public DateTime ErosketaData { get; set; }
        public int IdMintegia { get; set; }

        public Gailua(int id, string marka, string kokalekua, string egoera, DateTime erosketaData, int idMintegia = 1, string modeloa = "")
        {
            IdGailua = id;
            Marka = marka;
            Modeloa = modeloa;
            Kokalekua = kokalekua;
            Egoera = egoera;
            ErosketaData = erosketaData;
            IdMintegia = idMintegia;
        }

        public virtual string LortuInformazioa()
        {
            return $"ID: {IdGailua} | Marka: {Marka} | Kokalekua: {Kokalekua}";
        }
    }
}