using System;

namespace StockBaseApp.Modeloak
{
    /// <summary>
    /// <see cref="Gailua"/> klasearen kumea, inprimagailuen datu espezifikoak kudeatzeko.
    /// Klase honek koloretako inprimaketa-aukera gehitzen du.
    /// </summary>
    /// <param name="id">Gailuaren ID-a.</param>
    /// <param name="marka">Inprimagailuaren marka.</param>
    /// <param name="kokalekua">Gela edo kokapen fisikoa.</param>
    /// <param name="egoera">Uneko egoera (Aktiboa, Mantentze-lanetan...).</param>
    /// <param name="erosketaData">Erosketa data.</param>
    /// <param name="koloretakoa">Adierazten du inprimagailua koloretakoa den (true) edo ez (false).</param>
    public class Inprimagailua(int id, string marka, string kokalekua, string egoera, DateTime erosketaData, bool koloretakoa) 
        : Gailua(id, marka, kokalekua, egoera, erosketaData)
    {
        /// <summary>
        /// Inprimagailua koloretakoa den (Bai) edo ez (Ez) adierazten du.
        /// </summary>
        public bool Koloretakoa { get; set; } = koloretakoa;

        /// <summary>
        /// Inprimagailuaren informazio guztia testu formatuan itzultzen du, kolore aukera barne.
        /// </summary>
        /// <returns>Inprimagailuaren deskribapen osoa.</returns>
        public override string LortuInformazioa()
        {
            string colorTxt = Koloretakoa ? "Bai" : "Ez";
            return base.LortuInformazioa() + $" | Koloretakoa: {colorTxt}";
        }
    }
}