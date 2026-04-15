using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using StockBaseApp.Modeloak;

namespace StockBaseApp.Kontrolagailuak
{
    public class InbentarioSistema
    {
        private Konexioa konexioa;

        public InbentarioSistema()
        {
            konexioa = new Konexioa();
        }

        public Erabiltzailea? SaioaHasi(string email, string pasahitza)
        {
            using (var conn = konexioa.LortuKonexioa())
            {
                conn.Open();
                string sql = "SELECT id_erabiltzailea, izena, email, rola, id_mintegia FROM Erabiltzailea WHERE email = @email AND pasahitza = @pass";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@pass", pasahitza);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var user = new Erabiltzailea(
                                Convert.ToInt32(reader["id_erabiltzailea"]),
                                reader["izena"].ToString() ?? "",
                                reader["email"].ToString() ?? "",
                                reader["rola"].ToString() ?? ""
                            );
                            // Cargar ID del departamento del usuario
                            user.MintegiJabea = new Mintegia(Convert.IsDBNull(reader["id_mintegia"]) ? 1 : Convert.ToInt32(reader["id_mintegia"]), "");
                            return user;
                        }
                    }
                }
            }
            return null;
        }

        public void GailuaGehitu(Gailua gailua, int idKokalekua)
        {
            using (var conn = konexioa.LortuKonexioa())
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        string sqlGailua = "INSERT INTO Gailua (marka, modeloa, erosketa_data, egoera, id_mintegia, id_kokalekua) " +
                                           "VALUES (@marka, @modeloa, @data, @egoera, @mintegia, @kokalekua)";
                        int idGenerado = 0;
                        using (var cmd = new MySqlCommand(sqlGailua, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@marka", gailua.Marka);
                            cmd.Parameters.AddWithValue("@modeloa", gailua.Modeloa);
                            cmd.Parameters.AddWithValue("@data", gailua.ErosketaData);
                            cmd.Parameters.AddWithValue("@egoera", gailua.Egoera);
                            cmd.Parameters.AddWithValue("@mintegia", gailua.IdMintegia);
                            cmd.Parameters.AddWithValue("@kokalekua", idKokalekua);
                            cmd.ExecuteNonQuery();
                            idGenerado = (int)cmd.LastInsertedId;
                        }

                        if (gailua is Ordenagailua ordenador)
                        {
                            string sqlPC = "INSERT INTO Ordenagailua (gailua_id, prozesagailua, ram) VALUES (@id, @cpu, @ram)";
                            using (var cmd = new MySqlCommand(sqlPC, conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@id", idGenerado);
                                cmd.Parameters.AddWithValue("@cpu", ordenador.Prozesagailua);
                                cmd.Parameters.AddWithValue("@ram", ordenador.RamGB + "GB");
                                cmd.ExecuteNonQuery();
                            }
                        }
                        else if (gailua is Inprimagailua impresora)
                        {
                            string sqlPrint = "INSERT INTO Inprimagailua (gailua_id, koloretakoa) VALUES (@id, @color)";
                            using (var cmd = new MySqlCommand(sqlPrint, conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@id", idGenerado);
                                cmd.Parameters.AddWithValue("@color", impresora.Koloretakoa);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        trans.Commit();
                    }
                    catch (Exception) { trans.Rollback(); throw; }
                }
            }
        }

        public DataTable LortuGailuakGuztiak(int? idMintegia = null)
        {
            DataTable dt = new DataTable();
            using (var conn = konexioa.LortuKonexioa())
            {
                conn.Open();
                // SQL mejorado para incluir el nombre del Mintegia y permitir filtrado
                string whereClause = idMintegia.HasValue ? " WHERE g.id_mintegia = " + idMintegia.Value : "";
                
                string sql = $@"
                    SELECT g.id_gailua as ID, 'Ordenagailua' as Mota, g.marka as Marka, g.modeloa as Modeloa, 
                           m.izena as Mintegia, k.izena as Kokalekua, 
                           g.erosketa_data as Data, o.prozesagailua as CPU, o.ram as RAM, '' as Kolorea
                    FROM Gailua g 
                    INNER JOIN Ordenagailua o ON g.id_gailua = o.gailua_id
                    INNER JOIN Kokalekua k ON g.id_kokalekua = k.id_kokalekua
                    INNER JOIN Mintegia m ON g.id_mintegia = m.id_mintegia
                    {whereClause}
                    UNION ALL
                    SELECT g.id_gailua as ID, 'Inprimagailua' as Mota, g.marka as Marka, g.modeloa as Modeloa, 
                           m.izena as Mintegia, k.izena as Kokalekua, 
                           g.erosketa_data as Data, '' as CPU, '' as RAM, CASE WHEN i.koloretakoa = 1 THEN 'Bai' ELSE 'Ez' END as Kolorea
                    FROM Gailua g 
                    INNER JOIN Inprimagailua i ON g.id_gailua = i.gailua_id
                    INNER JOIN Kokalekua k ON g.id_kokalekua = k.id_kokalekua
                    INNER JOIN Mintegia m ON g.id_mintegia = m.id_mintegia
                    {whereClause}";
                
                using (var cmd = new MySqlCommand(sql, conn))
                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        public List<KeyValuePair<int, string>> LortuMintegiak()
        {
            var list = new List<KeyValuePair<int, string>>();
            using (var conn = konexioa.LortuKonexioa())
            {
                conn.Open();
                string sql = "SELECT id_mintegia, izena FROM Mintegia";
                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) list.Add(new KeyValuePair<int, string>(Convert.ToInt32(reader["id_mintegia"]), reader["izena"].ToString() ?? ""));
                }
            }
            return list;
        }

        public List<KeyValuePair<int, string>> LortuKokalekuak()
        {
            var list = new List<KeyValuePair<int, string>>();
            using (var conn = konexioa.LortuKonexioa())
            {
                conn.Open();
                string sql = "SELECT id_kokalekua, izena FROM Kokalekua";
                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) list.Add(new KeyValuePair<int, string>(Convert.ToInt32(reader["id_kokalekua"]), reader["izena"].ToString() ?? ""));
                }
            }
            return list;
        }

        public DataTable LortuEzabatutakoGailuak()
        {
            DataTable dt = new DataTable();
            using (var conn = konexioa.LortuKonexioa())
            {
                conn.Open();
                string sql = "SELECT id_ezabapena as 'ID Baja', id_gailua as 'ID Original', marka as Marka, " +
                             "erosketa_data as 'Erosketa Data', kokalekua as Kokalekua, ezabatze_data as 'Baja Data' " +
                             "FROM EzabatutakoGailua ORDER BY ezabatze_data DESC";
                using (var cmd = new MySqlCommand(sql, conn))
                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        public void GailuaEzabatu(Gailua gailua)
        {
            using (var conn = konexioa.LortuKonexioa())
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        string sqlHist = "INSERT INTO EzabatutakoGailua (id_gailua, marka, erosketa_data, kokalekua) VALUES (@id, @marka, @data, @kokalekua)";
                        using (var cmd = new MySqlCommand(sqlHist, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@id", gailua.IdGailua);
                            cmd.Parameters.AddWithValue("@marka", gailua.Marka);
                            cmd.Parameters.AddWithValue("@data", gailua.ErosketaData);
                            cmd.Parameters.AddWithValue("@kokalekua", gailua.Kokalekua);
                            cmd.ExecuteNonQuery();
                        }
                        string sqlDelSpec = gailua is Ordenagailua ? "DELETE FROM Ordenagailua WHERE gailua_id = @id" : "DELETE FROM Inprimagailua WHERE gailua_id = @id";
                        using (var cmd = new MySqlCommand(sqlDelSpec, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@id", gailua.IdGailua);
                            cmd.ExecuteNonQuery();
                        }
                        string sqlDelBase = "DELETE FROM Gailua WHERE id_gailua = @id";
                        using (var cmd = new MySqlCommand(sqlDelBase, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@id", gailua.IdGailua);
                            cmd.ExecuteNonQuery();
                        }
                        trans.Commit();
                    }
                    catch (Exception) { trans.Rollback(); throw; }
                }
            }
        }
    }
}