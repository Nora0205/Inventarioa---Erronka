using System;

namespace StockBaseApp.Modeloak
{
    public class EzabatutakoGailua(int id, string marka, string kokalekua, DateTime erosketa, DateTime ezabatze)
    {
        public int Id { get; set; } = id;
        public string Marka { get; set; } = marka;
        public string Kokalekua { get; set; } = kokalekua;
        public DateTime ErosketaData { get; set; } = erosketa;
        public DateTime EzabatzeData { get; set; } = ezabatze;
    }
}