using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using StockBaseApp.Modeloak;

namespace StockBaseApp.Kontrolagailuak
{
    /// <summary>
    /// Inbentarioaren kudeaketarako kontrolagailu nagusia.
    /// Datu-basearekiko operazio guztiak (CRUD) eta log-en kudeaketa zentralizatzen ditu.
    /// </summary>
    public class InbentarioSistema
    {
        private readonly Konexioa konexioa;

        /// <summary>
        /// InbentarioSistema klasearen instantzia berri bat sortzen du.
        /// </summary>
        public InbentarioSistema()
        {
            konexioa = new Konexioa();
        }

        /// <summary>
        /// Sisteman gertatzen diren ekintza garrantzitsuak 'Log' taulan gordetzen ditu.
        /// </summary>
        /// <param name="idErabiltzailea">Ekintza burutu duen erabiltzailearen ID-a.</param>
        /// <param name="ekintza">Burututako ekintza mota (INSERT, UPDATE, DELETE, LOGIN...).</param>
        /// <param name="taula">Aldatutako datu-baseko taularen izena.</param>
        /// <param name="idErregistroa">Aldatutako erregistroaren ID-a.</param>
        /// <param name="xehetasuna">Ekintzaren azalpen testual bat.</param>
        public void LogGorde(int idErabiltzailea, string ekintza, string taula, int idErregistroa, string xehetasuna)
        {
            using var kon = konexioa.LortuKonexioa();
            kon.Open();
            string sqlKatea = "INSERT INTO Log (id_erabiltzailea, ekintza, taula, id_erregistroa, xehetasuna) " +
                               "VALUES (@userId, @ekintza, @taula, @regId, @details)";
            using var komandoa = new MySqlCommand(sqlKatea, kon);
            komandoa.Parameters.AddWithValue("@userId", idErabiltzailea);
            komandoa.Parameters.AddWithValue("@ekintza", ekintza);
            komandoa.Parameters.AddWithValue("@taula", taula);
            komandoa.Parameters.AddWithValue("@regId", idErregistroa);
            komandoa.Parameters.AddWithValue("@details", xehetasuna);
            komandoa.ExecuteNonQuery();
        }

        /// <summary>
        /// Erabiltzailearen kredentzialak egiaztatzen ditu sisteman sartzeko.
        /// </summary>
        /// <param name="izena">Erabiltzaile izena.</param>
        /// <param name="pasahitza">Pasahitza (testu arrunta).</param>
        /// <returns>Erabiltzailea objektua kredentzialak zuzenak badira; null bestela.</returns>
        public Erabiltzailea? SaioaHasi(string izena, string pasahitza)
        {
            using var kon = konexioa.LortuKonexioa();
            kon.Open();
            string sqlKatea = @"
                SELECT e.id_erabiltzailea, e.izena, e.email, e.rola, e.id_mintegia, m.izena as mintegia_izena 
                FROM Erabiltzailea e 
                LEFT JOIN Mintegia m ON e.id_mintegia = m.id_mintegia 
                WHERE e.izena = @izena AND e.pasahitza = @pass";
            
            using var komandoa = new MySqlCommand(sqlKatea, kon);
            komandoa.Parameters.AddWithValue("@izena", izena);
            komandoa.Parameters.AddWithValue("@pass", pasahitza);
            using var irakurlea = komandoa.ExecuteReader();
            if (irakurlea.Read())
            {
                var erabiltzailea = new Erabiltzailea(
                    Convert.ToInt32(irakurlea["id_erabiltzailea"]),
                    irakurlea["izena"].ToString() ?? "",
                    irakurlea["email"].ToString() ?? "",
                    irakurlea["rola"].ToString() ?? ""
                )
                {
                    MintegiJabea = new Mintegia(
                        Convert.IsDBNull(irakurlea["id_mintegia"]) ? 1 : Convert.ToInt32(irakurlea["id_mintegia"]), 
                        irakurlea["mintegia_izena"]?.ToString() ?? ""
                    )
                };
                irakurlea.Close();
                LogGorde(erabiltzailea.IdErabiltzailea, "LOGIN", "Erabiltzailea", erabiltzailea.IdErabiltzailea, "Saioa hasi da sistema barruan.");
                return erabiltzailea;
            }
            return null;
        }

        /// <summary>
        /// Gailu berri bat gehitzen du inbentarioan, bere datu espezifikoekin (PC edo Inprimagailua).
        /// </summary>
        /// <param name="gailua">Gailu objektua (Ordenagailua edo Inprimagailua).</param>
        /// <param name="kokalekua">Gailuaren kokapen fisikoa (gela).</param>
        /// <param name="idErabiltzailea">Ekintza burutzen duen administratzailearen ID-a.</param>
        public void GailuaGehitu(Gailua gailua, string kokalekua, int idErabiltzailea)
        {
            using var kon = konexioa.LortuKonexioa();
            kon.Open();
            using var transakzioa = kon.BeginTransaction();
            try
            {
                string sqlGailua = "INSERT INTO Gailua (marka, modeloa, erosketa_data, egoera, id_mintegia, kokalekua) " +
                                   "VALUES (@marka, @modeloa, @data, @egoera, @mintegia, @kokalekua)";
                int sortutakoIda = 0;
                using (var komandoa = new MySqlCommand(sqlGailua, kon, transakzioa))
                {
                    komandoa.Parameters.AddWithValue("@marka", gailua.Marka);
                    komandoa.Parameters.AddWithValue("@modeloa", gailua.Modeloa);
                    komandoa.Parameters.AddWithValue("@data", gailua.ErosketaData);
                    komandoa.Parameters.AddWithValue("@egoera", gailua.Egoera);
                    komandoa.Parameters.AddWithValue("@mintegia", gailua.IdMintegia);
                    komandoa.Parameters.AddWithValue("@kokalekua", kokalekua);
                    komandoa.ExecuteNonQuery();
                    sortutakoIda = (int)komandoa.LastInsertedId;
                }

                if (gailua is Ordenagailua ordenagailua)
                {
                    string sqlPC = "INSERT INTO Ordenagailua (gailua_id, prozesagailua, ram) VALUES (@id, @cpu, @ram)";
                    using var komandoa = new MySqlCommand(sqlPC, kon, transakzioa);
                    komandoa.Parameters.AddWithValue("@id", sortutakoIda);
                    komandoa.Parameters.AddWithValue("@cpu", ordenagailua.Prozesagailua);
                    komandoa.Parameters.AddWithValue("@ram", ordenagailua.RamGB);
                    komandoa.ExecuteNonQuery();
                }
                else if (gailua is Inprimagailua inprimagailua)
                {
                    string sqlPrint = "INSERT INTO Inprimagailua (gailua_id, koloretakoa) VALUES (@id, @color)";
                    using var komandoa = new MySqlCommand(sqlPrint, kon, transakzioa);
                    komandoa.Parameters.AddWithValue("@id", sortutakoIda);
                    komandoa.Parameters.AddWithValue("@color", inprimagailua.Koloretakoa);
                    komandoa.ExecuteNonQuery();
                }

                transakzioa.Commit();
                LogGorde(idErabiltzailea, "INSERT", "Gailua", sortutakoIda, $"Gailu berria gehitu da: {gailua.Marka} {gailua.Modeloa} ({kokalekua} gelan)");
            }
            catch (Exception) { transakzioa.Rollback(); throw; }
        }

        /// <summary>
        /// Aktibo dauden gailu guztien zerrenda lortzen du, mintegi baten arabera iragazi daitekeena.
        /// </summary>
        /// <param name="idMintegia">Aukerakoa. Mintegi baten ID-a gailuak iragazteko.</param>
        /// <returns>Gailuen datuekin osatutako DataTable bat.</returns>
        public DataTable LortuGailuakGuztiak(int? idMintegia = null)
        {
            DataTable datuTaula = new();
            using var kon = konexioa.LortuKonexioa();
            kon.Open();
            string nonBaldintza = idMintegia.HasValue ? " AND g.id_mintegia = " + idMintegia.Value : "";

            string sqlKatea = $@"
                    SELECT g.id_gailua as Identifikatzailea, 'Ordenagailua' as Mota, g.marka as Marka, g.modeloa as Modeloa, 
                           m.izena as Mintegia, g.kokalekua as Kokalekua, g.egoera as Egoera,
                           g.erosketa_data as Data, o.prozesagailua as CPU, o.ram as RAM, '' as Kolorea
                    FROM Gailua g 
                    INNER JOIN Ordenagailua o ON g.id_gailua = o.gailua_id
                    INNER JOIN Mintegia m ON g.id_mintegia = m.id_mintegia
                    WHERE g.egoera = 'Aktiboa' {nonBaldintza}
                    UNION ALL
                    SELECT g.id_gailua as Identifikatzailea, 'Inprimagailua' as Mota, g.marka as Marka, g.modeloa as Modeloa, 
                           m.izena as Mintegia, g.kokalekua as Kokalekua, g.egoera as Egoera,
                           g.erosketa_data as Data, '' as CPU, '' as RAM, CASE WHEN i.koloretakoa = 1 THEN 'Bai' ELSE 'Ez' END as Kolorea
                    FROM Gailua g 
                    INNER JOIN Inprimagailua i ON g.id_gailua = i.gailua_id
                    INNER JOIN Mintegia m ON g.id_mintegia = m.id_mintegia
                    WHERE g.egoera = 'Aktiboa' {nonBaldintza}";

            using var komandoa = new MySqlCommand(sqlKatea, kon);
            using var egokitzailea = new MySqlDataAdapter(komandoa);
            egokitzailea.Fill(datuTaula);
            return datuTaula;
        }

        /// <summary>
        /// Mantentze-lanetan, hautsita edo ezabatuta dauden gailuen zerrenda konbinatua lortzen du.
        /// </summary>
        /// <returns>Baja eta mantentze sistemaren datuekin osatutako DataTable bat.</returns>
        public DataTable LortuBajaEtaMantentzeSistema()
        {
            DataTable datuTaula = new();
            using var kon = konexioa.LortuKonexioa();
            kon.Open();
            string sqlKatea = @"
                SELECT id_gailua as Identifikatzailea, marka as Marka, kokalekua as Kokalekua, egoera as Egoera, erosketa_data as 'Erosketa Data', NULL as 'Ezabatze Data'
                FROM Gailua 
                WHERE egoera != 'Aktiboa'
                UNION ALL
                SELECT id_gailua as Identifikatzailea, marka as Marka, kokalekua as Kokalekua, 'EZABATUA' as Egoera, erosketa_data as 'Erosketa Data', ezabatze_data as 'Ezabatze Data'
                FROM EzabatutakoGailua
                ORDER BY Egoera DESC";

            using var komandoa = new MySqlCommand(sqlKatea, kon);
            using var egokitzailea = new MySqlDataAdapter(komandoa);
            egokitzailea.Fill(datuTaula);
            return datuTaula;
        }

        /// <summary>
        /// Sisteman erregistratuta dauden mintegi guztiak lortzen ditu.
        /// </summary>
        /// <returns>ID eta Izena bikoteen zerrenda bat.</returns>
        public List<KeyValuePair<int, string>> LortuMintegiak()
        {
            var zerrenda = new List<KeyValuePair<int, string>>();
            using var kon = konexioa.LortuKonexioa();
            kon.Open();
            string sqlKatea = "SELECT id_mintegia, izena FROM Mintegia";
            using var komandoa = new MySqlCommand(sqlKatea, kon);
            using var irakurlea = komandoa.ExecuteReader();
            while (irakurlea.Read())
                zerrenda.Add(new(Convert.ToInt32(irakurlea["id_mintegia"]), irakurlea["izena"].ToString() ?? ""));
            return zerrenda;
        }

        /// <summary>
        /// Sisteman erregistratuta dauden kokaleku (gela) guztiak lortzen ditu.
        /// </summary>
        /// <returns>ID eta Izena bikoteen zerrenda bat.</returns>
        public List<KeyValuePair<int, string>> LortuKokalekuak()
        {
            var zerrenda = new List<KeyValuePair<int, string>>();
            using var kon = konexioa.LortuKonexioa();
            kon.Open();
            string sqlKatea = "SELECT id_kokalekua, izena FROM Kokalekua";
            using var komandoa = new MySqlCommand(sqlKatea, kon);
            using var irakurlea = komandoa.ExecuteReader();
            while (irakurlea.Read())
                zerrenda.Add(new(Convert.ToInt32(irakurlea["id_kokalekua"]), irakurlea["izena"].ToString() ?? ""));
            return zerrenda;
        }

        /// <summary>
        /// Gailu bat inbentariotik guztiz ezabatzen du. 
        /// MySQL-ko TRIGGER batek automatikoki gordeko du gailua 'EzabatutakoGailua' taulan.
        /// </summary>
        /// <param name="gailua">Ezabatu nahi den gailu objektua.</param>
        /// <param name="idErabiltzailea">Ekintza burutzen duen administratzailearen ID-a.</param>
        public void GailuaEzabatu(Gailua gailua, int idErabiltzailea)
        {
            using var kon = konexioa.LortuKonexioa();
            kon.Open();
            using var transakzioa = kon.BeginTransaction();
            try
            {
                // Lehen, hemen 'EzabatutakoGailua' taulan txertatzen genuen eskuz.
                // Orain, MySQL-ko TRIGGER-ak (HistorialBorrados_Trigger) automatikoki egiten du.

                // Ezabatu datu espezifikoak (Ordenagailua edo Inprimagailua)
                string sqlDelSpec = gailua is Ordenagailua ? "DELETE FROM Ordenagailua WHERE gailua_id = @id" : "DELETE FROM Inprimagailua WHERE gailua_id = @id";
                using (var komandoa = new MySqlCommand(sqlDelSpec, kon, transakzioa))
                {
                    komandoa.Parameters.AddWithValue("@id", gailua.IdGailua);
                    komandoa.ExecuteNonQuery();
                }

                // Ezabatu gailu nagusia. Honek 'HistorialBorrados_Trigger' aktibatuko du.
                string sqlDelBase = "DELETE FROM Gailua WHERE id_gailua = @id";
                using (var komandoa = new MySqlCommand(sqlDelBase, kon, transakzioa))
                {
                    komandoa.Parameters.AddWithValue("@id", gailua.IdGailua);
                    komandoa.ExecuteNonQuery();
                }

                transakzioa.Commit();
                LogGorde(idErabiltzailea, "DELETE", "Gailua", gailua.IdGailua, $"Gailua guztiz ezabatu da: {gailua.Marka} {gailua.Modeloa}.");
            }
            catch (Exception) { transakzioa.Rollback(); throw; }
        }

        /// <summary>
        /// Biltegiratutako prozedura bat deitzen du (Stored Procedure), 10 gailu zaharrenak lortzeko.
        /// </summary>
        /// <returns>10 gailu zaharrenen datuekin osatutako DataTable bat.</returns>
        public DataTable Lortu10GailuZaharrenak()
        {
            DataTable datuTaula = new();
            using var kon = konexioa.LortuKonexioa();
            kon.Open();
            
            // Biltegiratutako prozedura deitzen dugu
            using var komandoa = new MySqlCommand("Lortu10Zaharrenak", kon);
            komandoa.CommandType = CommandType.StoredProcedure;
            
            using var egokitzailea = new MySqlDataAdapter(komandoa);
            egokitzailea.Fill(datuTaula);
            return datuTaula;
        }

        /// <summary>
        /// Erabiltzaile berri bat sortzen du sisteman.
        /// </summary>
        /// <param name="izena">Erabiltzaile izen osoa.</param>
        /// <param name="email">Posta elektronikoa.</param>
        /// <param name="pasahitza">Sarbide pasahitza.</param>
        /// <param name="rola">Sistema rola (IKT, Irakaslea, Administratzailea...).</param>
        /// <param name="idMintegia">Erabiltzailea zein mintegikoa den.</param>
        /// <param name="idAdmin">Ekintza burutzen duen administratzailearen ID-a.</param>
        public void ErabiltzaileaGehitu(string izena, string email, string pasahitza, string rola, int idMintegia, int idAdmin)
        {
            using var kon = konexioa.LortuKonexioa();
            kon.Open();
            string sqlKatea = "INSERT INTO Erabiltzailea (izena, email, pasahitza, rola, id_mintegia) VALUES (@izena, @email, @pass, @rola, @mintegia)";
            using var komandoa = new MySqlCommand(sqlKatea, kon);
            komandoa.Parameters.AddWithValue("@izena", izena);
            komandoa.Parameters.AddWithValue("@email", email);
            komandoa.Parameters.AddWithValue("@pass", pasahitza);
            komandoa.Parameters.AddWithValue("@rola", rola);
            komandoa.Parameters.AddWithValue("@mintegia", idMintegia);
            komandoa.ExecuteNonQuery();
            int nuevoId = (int)komandoa.LastInsertedId;
            LogGorde(idAdmin, "INSERT", "Erabiltzailea", nuevoId, $"Erabiltzaile berria sortu da: {izena} ({rola})");
        }

        public void ErabiltzaileaEzabatu(int id, string izena, int idAdmin)
        {
            if (izena.Equals("Jon Agirretxe", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Ezin da IKT arduradun nagusia ezabatu.");
            }

            using var kon = konexioa.LortuKonexioa();
            kon.Open();
            string sqlKatea = "DELETE FROM Erabiltzailea WHERE id_erabiltzailea = @id";
            using var komandoa = new MySqlCommand(sqlKatea, kon);
            komandoa.Parameters.AddWithValue("@id", id);
            komandoa.ExecuteNonQuery();
            LogGorde(idAdmin, "DELETE", "Erabiltzailea", id, $"Erabiltzailea ezabatu da: {izena}");
        }

        public DataTable ErabiltzaileGuztiakLortu()
        {
            DataTable datuTaula = new();
            using var kon = konexioa.LortuKonexioa();
            kon.Open();
            string sqlKatea = @"
                SELECT e.id_erabiltzailea as Identifikatzailea, e.izena as Izena, e.email as Email, 
                       e.rola as Rola, m.izena as Mintegia 
                FROM Erabiltzailea e
                INNER JOIN Mintegia m ON e.id_mintegia = m.id_mintegia";
            using var komandoa = new MySqlCommand(sqlKatea, kon);
            using var egokitzailea = new MySqlDataAdapter(komandoa);
            egokitzailea.Fill(datuTaula);
            return datuTaula;
        }

        public int ErabiltzaileKopuruaLortu(string rola, int? idMintegia = null)
        {
            using var kon = konexioa.LortuKonexioa();
            kon.Open();
            string sqlKatea = "SELECT COUNT(*) FROM Erabiltzailea WHERE rola = @rola";
            if (idMintegia.HasValue) sqlKatea += " AND id_mintegia = @mintegia";

            using var komandoa = new MySqlCommand(sqlKatea, kon);
            komandoa.Parameters.AddWithValue("@rola", rola);
            if (idMintegia.HasValue) komandoa.Parameters.AddWithValue("@mintegia", idMintegia.Value);

            return Convert.ToInt32(komandoa.ExecuteScalar());
        }

        public void GailuaEgoeraAldatu(int id, string egoeraBerria, int idErabiltzailea)
        {
            using var kon = konexioa.LortuKonexioa();
            kon.Open();

            string kokalekuaBerria = "";
            if (egoeraBerria == "Mantentze-lanetan" || egoeraBerria == "Hautsia")
            {
                kokalekuaBerria = ", kokalekua = 'IKT Tailerra'";
            }

            string sqlKatea = $"UPDATE Gailua SET egoera = @egoera {kokalekuaBerria} WHERE id_gailua = @id";
            using var komandoa = new MySqlCommand(sqlKatea, kon);
            komandoa.Parameters.AddWithValue("@egoera", egoeraBerria);
            komandoa.Parameters.AddWithValue("@id", id);
            komandoa.ExecuteNonQuery();
            LogGorde(idErabiltzailea, "UPDATE", "Gailua", id, $"Egoera aldatu da: {egoeraBerria}.");
        }

        public void GailuaKokalekuaAldatu(int id, string kokalekua, int idErabiltzailea)
        {
            using var kon = konexioa.LortuKonexioa();
            kon.Open();
            string sqlKatea = "UPDATE Gailua SET kokalekua = @kokalekua WHERE id_gailua = @id";
            using var komandoa = new MySqlCommand(sqlKatea, kon);
            komandoa.Parameters.AddWithValue("@kokalekua", kokalekua);
            komandoa.Parameters.AddWithValue("@id", id);
            komandoa.ExecuteNonQuery();
            LogGorde(idErabiltzailea, "UPDATE", "Gailua", id, $"Kokalekua aldatu da: {kokalekua}.");
        }

        public void MintegiaEzabatu(int idMintegia, int idAdmin)
        {
            if (idMintegia == 1 || idMintegia == 99)
                throw new Exception("Ezin da mintegi sistema hau ezabatu.");

            int irakasleKopurua = ErabiltzaileKopuruaLortu("Irakaslea", idMintegia);
            if (irakasleKopurua > 0)
                throw new Exception("Ezin da mintegia ezabatu irakasleren bat duelako.");

            using var kon = konexioa.LortuKonexioa();
            kon.Open();
            using var transakzioa = kon.BeginTransaction();
            try
            {
                // Gailuak "Birbanatu" (99) mintegira mugitu
                string sqlGailuak = "UPDATE Gailua SET id_mintegia = 99 WHERE id_mintegia = @id";
                using (var komandoa = new MySqlCommand(sqlGailuak, kon, transakzioa))
                {
                    komandoa.Parameters.AddWithValue("@id", idMintegia);
                    komandoa.ExecuteNonQuery();
                }

                // Erabiltzaileak "Informatika" (1) mintegira mugitu
                string sqlErabiltzaileak = "UPDATE Erabiltzailea SET id_mintegia = 1 WHERE id_mintegia = @id";
                using (var komandoa = new MySqlCommand(sqlErabiltzaileak, kon, transakzioa))
                {
                    komandoa.Parameters.AddWithValue("@id", idMintegia);
                    komandoa.ExecuteNonQuery();
                }

                // Mintegia ezabatu
                string sqlMintegia = "DELETE FROM Mintegia WHERE id_mintegia = @id";
                using (var komandoa = new MySqlCommand(sqlMintegia, kon, transakzioa))
                {
                    komandoa.Parameters.AddWithValue("@id", idMintegia);
                    komandoa.ExecuteNonQuery();
                }

                transakzioa.Commit();
                LogGorde(idAdmin, "DELETE", "Mintegia", idMintegia, "Mintegia ezabatu da eta gailuak birbanatu dira.");
            }
            catch (Exception) { transakzioa.Rollback(); throw; }
        }

        /// <summary>
        /// Mintegi berri bat gehitzen du sisteman.
        /// </summary>
        /// <param name="izena">Mintegiaren izena.</param>
        /// <param name="idAdmin">Ekintza burutzen duen administratzailearen ID-a.</param>
        public void MintegiaGehitu(string izena, int idAdmin)
        {
            if (string.IsNullOrWhiteSpace(izena))
                throw new Exception("Izena ezin da hutsik egon.");

            using var kon = konexioa.LortuKonexioa();
            kon.Open();

            // Hurrengo ID-a eskuz lortu, ez baita auto-inkrementala
            string sqlMaxId = "SELECT IFNULL(MAX(id_mintegia), 0) + 1 FROM Mintegia WHERE id_mintegia < 99";
            using var cmdId = new MySqlCommand(sqlMaxId, kon);
            int nuevoId = Convert.ToInt32(cmdId.ExecuteScalar());

            string sqlInsert = "INSERT INTO Mintegia (id_mintegia, izena) VALUES (@id, @izena)";
            using var komandoa = new MySqlCommand(sqlInsert, kon);
            komandoa.Parameters.AddWithValue("@id", nuevoId);
            komandoa.Parameters.AddWithValue("@izena", izena);
            komandoa.ExecuteNonQuery();

            LogGorde(idAdmin, "INSERT", "Mintegia", nuevoId, $"Mintegi berria sortu da: {izena}");
        }
    }
}