using System;

namespace StockBaseApp.Modeloak
{

    /// <summary>
    /// Inbentarioko gailu guztien oinarrizko klasea.
    /// Klase honek gailu guztiek komunean dituzten atributuak eta portaerak definitzen ditu.
    /// </summary>
    public class Gailua
    {
        /// <summary>
        /// Gailuaren identifikatzaile esklusiboa datu-basean.
        /// </summary>
        public int IdGailua { get; set; }
        
        /// <summary>
        /// Gailuaren marka komertziala (adibidez: Dell, HP, Epson).
        /// </summary>
        public string Marka { get; set; }

        /// <summary>
        /// Gailuaren modelo espezifikoa.
        /// </summary>
        public string Modeloa { get; set; }
        
        /// <summary>
        /// Gailuaren kokapen fisikoa edo gela (adibidez: 2.1 gela, IKT Tailerra).
        /// </summary>
        public string Kokalekua { get; set; }
        
        /// <summary>
        /// Gailuaren uneko egoera (Aktiboa, Mantentze-lanetan, Hautsia...).
        /// </summary>
        public string Egoera { get; set; }
        
        /// <summary>
        /// Gailua erosi zeneko data. Antzinatasuna kalkulatzeko erabiltzen da.
        /// </summary>
        public DateTime ErosketaData { get; set; }
        
        /// <summary>
        /// Gailua zein mintegiren jabetza den adierazten duen ID-a.
        /// </summary>
        public int IdMintegia { get; set; }

        /// <summary>
        /// Gailu objektu berri bat sortzen du bere oinarrizko datuekin.
        /// </summary>
        /// <param name="id">Gailuaren ID-a.</param>
        /// <param name="marka">Gailuaren marka.</param>
        /// <param name="kokalekua">Gela edo kokapena.</param>
        /// <param name="egoera">Uneko egoera.</param>
        /// <param name="erosketaData">Erosketa data.</param>
        /// <param name="idMintegia">Jabe den mintegiaren ID-a (lehenetsia: 1).</param>
        /// <param name="modeloa">Gailuaren modeloa (lehenetsia: "").</param>
        public Gailua(int id, string marka, string kokalekua, string egoera, DateTime erosketaData, int idMintegia = 1, string modeloa = "")
        {
            IdGailua = id;
            Marka = marka;
            Kokalekua = kokalekua;
            Egoera = egoera;
            ErosketaData = erosketaData;
            IdMintegia = idMintegia;
            Modeloa = modeloa;
        }

        /// <summary>
        /// Gailuaren oinarrizko informazioa testu formatuan itzultzen du.
        /// Klase kumeek gainidatzi dezakete (override) informazio gehigarria emateko.
        /// </summary>
        /// <returns>Gailuaren deskribapen laburra.</returns>
        public virtual string LortuInformazioa()
        {
            return $"ID: {IdGailua} | Marka: {Marka} | Kokalekua: {Kokalekua}";
        }
    }
}