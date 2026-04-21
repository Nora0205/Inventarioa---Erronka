using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using StockBaseApp.Modeloak;

namespace StockBaseApp.Kontrolagailuak
{
    public class InbentarioSistema
    {
        private readonly Konexioa konexioa;

        public InbentarioSistema()
        {
            konexioa = new Konexioa();
        }

        public Erabiltzailea? SaioaHasi(string izena, string pasahitza)
        {
            using var kon = konexioa.LortuKonexioa();
            kon.Open();
            string sqlKatea = "SELECT id_erabiltzailea, izena, email, rola, id_mintegia FROM Erabiltzailea WHERE izena = @izena AND pasahitza = @pass";
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
                    MintegiJabea = new Mintegia(Convert.IsDBNull(irakurlea["id_mintegia"]) ? 1 : Convert.ToInt32(irakurlea["id_mintegia"]), "")
                };
                return erabiltzailea;
            }
            return null;
        }

        public void GailuaGehitu(Gailua gailua, string kokalekua)
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
            }
            catch (Exception) { transakzioa.Rollback(); throw; }
        }

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

        public void GailuaEzabatu(Gailua gailua)
        {
            using var kon = konexioa.LortuKonexioa();
            kon.Open();
            using var transakzioa = kon.BeginTransaction();
            try
            {
                string sqlHist = "INSERT INTO EzabatutakoGailua (id_gailua, marka, erosketa_data, kokalekua) VALUES (@id, @marka, @data, @kokalekua)";
                using (var komandoa = new MySqlCommand(sqlHist, kon, transakzioa))
                {
                    komandoa.Parameters.AddWithValue("@id", gailua.IdGailua);
                    komandoa.Parameters.AddWithValue("@marka", gailua.Marka);
                    komandoa.Parameters.AddWithValue("@data", gailua.ErosketaData);
                    komandoa.Parameters.AddWithValue("@kokalekua", gailua.Kokalekua);
                    komandoa.ExecuteNonQuery();
                }
                string sqlDelSpec = gailua is Ordenagailua ? "DELETE FROM Ordenagailua WHERE gailua_id = @id" : "DELETE FROM Inprimagailua WHERE gailua_id = @id";
                using (var komandoa = new MySqlCommand(sqlDelSpec, kon, transakzioa))
                {
                    komandoa.Parameters.AddWithValue("@id", gailua.IdGailua);
                    komandoa.ExecuteNonQuery();
                }
                string sqlDelBase = "DELETE FROM Gailua WHERE id_gailua = @id";
                using (var komandoa = new MySqlCommand(sqlDelBase, kon, transakzioa))
                {
                    komandoa.Parameters.AddWithValue("@id", gailua.IdGailua);
                    komandoa.ExecuteNonQuery();
                }
                transakzioa.Commit();
            }
            catch (Exception) { transakzioa.Rollback(); throw; }
        }

        public void ErabiltzaileaGehitu(string izena, string email, string pasahitza, string rola, int idMintegia)
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
        }

        public void ErabiltzaileaEzabatu(int id, string izena)
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

        public void GailuaEgoeraAldatu(int id, string egoeraBerria)
        {
            using var kon = konexioa.LortuKonexioa();
            kon.Open();
            
            // Hautsia bada, IKT Tailerrera mugitu automatikoki
            string kokalekuaBerria = "";
            if (egoeraBerria == "Mantentze-lanetan" || egoeraBerria == "Hautsia")
            {
                kokalekuaBerria = ", kokalekua = 'IKT Tailerra'";
            }
            // Aktibatu bada (berreskuratu), EZ dugu kokalekua aldatuko, IKT Tailerretan utziko dugu

            string sqlKatea = $"UPDATE Gailua SET egoera = @egoera {kokalekuaBerria} WHERE id_gailua = @id";
            using var komandoa = new MySqlCommand(sqlKatea, kon);
            komandoa.Parameters.AddWithValue("@egoera", egoeraBerria);
            komandoa.Parameters.AddWithValue("@id", id);
            komandoa.ExecuteNonQuery();
        }

        public void GailuaKokalekuaAldatu(int id, string kokalekua)
        {
            using var kon = konexioa.LortuKonexioa();
            kon.Open();
            string sqlKatea = "UPDATE Gailua SET kokalekua = @kokalekua WHERE id_gailua = @id";
            using var komandoa = new MySqlCommand(sqlKatea, kon);
            komandoa.Parameters.AddWithValue("@kokalekua", kokalekua);
            komandoa.Parameters.AddWithValue("@id", id);
            komandoa.ExecuteNonQuery();
        }
    }
}