using System.Collections.Generic;

namespace StockBaseApp.Modeloak
{
    /// <summary>
    /// Sistemako erabiltzaile baten informazioa eta baimenak kudeatzen dituen klasea.
    /// Erabiltzaile bakoitza mintegi (departamentu) bati lotuta dago eta rol bat du.
    /// </summary>
    /// <param name="id">Erabiltzailearen ID esklusiboa.</param>
    /// <param name="izena">Erabiltzailearen izen osoa.</param>
    /// <param name="email">Posta elektronikoa (sistemako jakinarazpenetarako).</param>
    /// <param name="rola">Sistema barruko rola (Adib: IKT Arduraduna, Irakaslea, Administratzailea).</param>
    /// <param name="pasahitza">Sarbide pasahitza (hautazkoa eraikitzailean).</param>
    public class Erabiltzailea(int id, string izena, string email, string rola, string pasahitza = "")
    {
        /// <summary>
        /// Erabiltzailearen identifikatzailea datu-basean.
        /// </summary>
        public int IdErabiltzailea { get; set; } = id;

        /// <summary>
        /// Erabiltzailearen izen-abizenak.
        /// </summary>
        public string Izena { get; set; } = izena;

        /// <summary>
        /// Erabiltzailearen helbide elektronikoa.
        /// </summary>
        public string Email { get; set; } = email;

        /// <summary>
        /// Hash bidez edo testu arruntean gordetako pasahitza.
        /// </summary>
        public string Pasahitza { get; set; } = pasahitza;

        /// <summary>
        /// Erabiltzailearen funtzioa sisteman. Baimenak kontrolatzeko erabiltzen da.
        /// </summary>
        public string Rola { get; set; } = rola;

        /// <summary>
        /// Erabiltzailea zein mintegiren partaide den adierazten du.
        /// <see cref="Mintegia"/> objektu bati erreferentzia egiten dio.
        /// </summary>
        public Mintegia? MintegiJabea { get; set; }
    }
}