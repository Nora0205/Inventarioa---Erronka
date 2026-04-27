using System.Collections.Generic;

namespace StockBaseApp.Modeloak
{
    /// <summary>
    /// Ikastetxeko mintegi edo departamentu bat ordezkatzen duen klasea.
    /// Mintegi bakoitzak bere gailu inbentario propioa kudeatzen du.
    /// </summary>
    /// <param name="id">Mintegiaren ID esklusiboa.</param>
    /// <param name="izena">Mintegiaren izena (Adib: Informatika, Hizkuntzak, Elektrizitatea).</param>
    public class Mintegia(int id, string izena)
    {
        /// <summary>
        /// Mintegiaren identifikatzailea datu-basean.
        /// </summary>
        public int IdMintegia { get; set; } = id;

        /// <summary>
        /// Mintegiaren izen deskriptiboa.
        /// </summary>
        public string Izena { get; set; } = izena;

        /// <summary>
        /// Mintegi honi lotuta dauden gailu guztien zerrenda.
        /// <see cref="Gailua"/> motako objektuak gordetzen ditu.
        /// </summary>
        public List<Gailua> Gailuak { get; set; } = [];
    }
}