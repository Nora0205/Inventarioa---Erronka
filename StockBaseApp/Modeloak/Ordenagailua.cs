using System;

namespace StockBaseApp.Modeloak
{
    public class Ordenagailua(int id, string marka, string kokalekua, string egoera, DateTime erosketaData, string prozesagailua, int ram) 
        : Gailua(id, marka, kokalekua, egoera, erosketaData)
    {
        public string Prozesagailua { get; set; } = prozesagailua;
        public int RamGB { get; set; } = ram;

        public override string LortuInformazioa()
        {
            return base.LortuInformazioa() + $" | CPU: {Prozesagailua} | RAM: {RamGB}GB";
        }
    }
}