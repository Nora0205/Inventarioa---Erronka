using System;

namespace StockBaseApp.Modeloak
{
    /// <summary>
    /// <see cref="Gailua"/> klasearen kumea, ordenagailuen datu espezifikoak kudeatzeko.
    /// Klase honek prozesagailuaren eta RAM memoriaren informazioa gehitzen du.
    /// </summary>
    /// <param name="id">Gailuaren ID-a.</param>
    /// <param name="marka">Ordenagailuaren marka.</param>
    /// <param name="kokalekua">Gela edo kokapen fisikoa.</param>
    /// <param name="egoera">Uneko egoera (Aktiboa, Mantentze-lanetan...).</param>
    /// <param name="erosketaData">Erosketa data.</param>
    /// <param name="prozesagailua">Ordenagailuak duen prozesagailu modeloa.</param>
    /// <param name="ram">RAM memoria kopurua GB-tan.</param>
    public class Ordenagailua(int id, string marka, string kokalekua, string egoera, DateTime erosketaData, string prozesagailua, int ram) 
        : Gailua(id, marka, kokalekua, egoera, erosketaData)
    {
        /// <summary>
        /// Ordenagailuaren prozesagailua (CPU). Adibidez: Intel i5, AMD Ryzen 7.
        /// </summary>
        public string Prozesagailua { get; set; } = prozesagailua;

        /// <summary>
        /// Ordenagailuaren RAM memoria kopurua Gigabyte-tan (GB).
        /// </summary>
        public int RamGB { get; set; } = ram;

        /// <summary>
        /// Ordenagailuaren informazio guztia testu formatuan itzultzen du, CPU eta RAM barne.
        /// </summary>
        /// <returns>Ordenagailuaren deskribapen osoa.</returns>
        public override string LortuInformazioa()
        {
            return base.LortuInformazioa() + $" | CPU: {Prozesagailua} | RAM: {RamGB}GB";
        }
    }
}