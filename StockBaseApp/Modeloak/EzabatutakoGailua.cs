using System;

namespace StockBaseApp.Modeloak
{
    public class EzabatutakoGailua
    {
        public int Id { get; set; }
        public string Marka { get; set; } = "";
        public string Kokalekua { get; set; } = "";
        public DateTime ErosketaData { get; set; }
        public DateTime EzabatzeData { get; set; }

        public EzabatutakoGailua(int id, string marka, string kokalekua, DateTime erosketa, DateTime ezabatze)
        {
            Id = id;
            Marka = marka;
            Kokalekua = kokalekua;
            ErosketaData = erosketa;
            EzabatzeData = ezabatze;
        }
    }
}