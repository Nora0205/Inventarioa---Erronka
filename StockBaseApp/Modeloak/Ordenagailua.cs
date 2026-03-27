using System;

namespace StockBaseApp.Modeloak
{
    public class Ordenagailua : Gailua
    {
        public string Prozesagailua { get; set; } = "";
        public int RamGB { get; set; }

        public Ordenagailua(int id, string marka, string kokalekua, string egoera, DateTime erosketaData, string prozesagailua, int ram)
            : base(id, marka, kokalekua, egoera, erosketaData)
        {
            Prozesagailua = prozesagailua;
            RamGB = ram;
        }

        public override string LortuInformazioa()
        {
            return base.LortuInformazioa() + $" | CPU: {Prozesagailua} | RAM: {RamGB}GB";
        }
    }
}