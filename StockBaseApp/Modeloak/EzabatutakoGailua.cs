using System;

namespace StockBaseApp.Modeloak
{
    /// <summary>
    /// Inbentariotik ezabatu diren gailuen informazio historikoa gordetzeko modeloa.
    /// MySQL-ko TRIGGER-ak taula honetan txertatzen ditu datuak automatikoki gailu bat ezabatzean.
    /// </summary>
    /// <param name="id">Jatorrizko gailuaren ID-a.</param>
    /// <param name="marka">Gailuaren marka.</param>
    /// <param name="kokalekua">Gailua ezabatu aurretik zegoen gela.</param>
    /// <param name="erosketa">Gailuaren jatorrizko erosketa data.</param>
    /// <param name="ezabatze">Gailua sistematik ezabatu zeneko data.</param>
    public class EzabatutakoGailua(int id, string marka, string kokalekua, DateTime erosketa, DateTime ezabatze)
    {
        /// <summary>
        /// Gailuaren identifikatzailea.
        /// </summary>
        public int Id { get; set; } = id;

        /// <summary>
        /// Gailuaren marka.
        /// </summary>
        public string Marka { get; set; } = marka;

        /// <summary>
        /// Gailuaren azken kokapena ezabatu aurretik.
        /// </summary>
        public string Kokalekua { get; set; } = kokalekua;

        /// <summary>
        /// Gailuaren jatorrizko erosketa data.
        /// </summary>
        public DateTime ErosketaData { get; set; } = erosketa;

        /// <summary>
        /// Sisteman gailuaren baja eman zeneko data espezifikoa.
        /// </summary>
        public DateTime EzabatzeData { get; set; } = ezabatze;
    }
}