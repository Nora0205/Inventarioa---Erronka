using System.Collections.Generic;
using StockBaseApp.Modeloak;

namespace StockBaseApp.Kontrolagailuak
{
    public class InbentarioKudeatzailea
    {
        private List<Gailua> gailuenZerrenda;

        public InbentarioKudeatzailea()
        {
            gailuenZerrenda = new List<Gailua>();
        }

        public void GailuaGehitu(Gailua gailuBerria)
        {
            gailuenZerrenda.Add(gailuBerria);
        }

        public List<Gailua> LortuGailuak()
        {
            return gailuenZerrenda;
        }
    }
}