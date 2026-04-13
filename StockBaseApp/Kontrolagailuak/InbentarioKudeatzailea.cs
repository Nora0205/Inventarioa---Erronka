using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using StockBaseApp.Modeloak;

namespace StockBaseApp.Kontrolagailuak
{
    public class InbentarioKudeatzailea
    {
        private Konexioa konexioa;

        public InbentarioKudeatzailea()
        {
            konexioa = new Konexioa();
        }

        public Erabiltzailea? SaioaHasi(string email, string pasahitza)
        {
            using (var conn = konexioa.LortuKonexioa())
            {
                conn.Open();
                string sql = "SELECT id_erabiltzailea, izena, email, rola FROM Erabiltzailea WHERE email = @email AND pasahitza = @pass";
                
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@pass", pasahitza);
                    
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Erabiltzailea(
                                Convert.ToInt32(reader["id_erabiltzailea"]),
                                reader["izena"].ToString() ?? "",
                                reader["email"].ToString() ?? "",
                                reader["rola"].ToString() ?? ""
                            );
                        }
                    }
                }
            }
            return null;
        }

        public List<KeyValuePair<int, string>> LortuKokalekuak()
        {
            var kokalekuak = new List<KeyValuePair<int, string>>();
            using (var conn = konexioa.LortuKonexioa())
            {
                conn.Open();
                string sql = "SELECT id_kokalekua, izena FROM Kokalekua";
                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        kokalekuak.Add(new KeyValuePair<int, string>(
                            Convert.ToInt32(reader["id_kokalekua"]),
                            reader["izena"].ToString() ?? ""
                        ));
                    }
                }
            }
            return kokalekuak;
        }

        public List<KeyValuePair<int, string>> LortuMintegiak()
        {
            var mintegiak = new List<KeyValuePair<int, string>>();
            using (var conn = konexioa.LortuKonexioa())
            {
                conn.Open();
                string sql = "SELECT id_mintegia, izena FROM Mintegia";
                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        mintegiak.Add(new KeyValuePair<int, string>(
                            Convert.ToInt32(reader["id_mintegia"]),
                            reader["izena"].ToString() ?? ""
                        ));
                    }
                }
            }
            return mintegiak;
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
                        // 1. Txertatu Gailua taulan (ID gabe, id_kokalekua erabiliz)
                        string sqlGailua = "INSERT INTO Gailua (marka, erosketa_data, egoera, id_mintegia, id_kokalekua) " +
                                           "VALUES (@marka, @data, @egoera, @mintegia, @kokalekua)";
                        
                        int idGenerado = 0;
                        using (var cmd = new MySqlCommand(sqlGailua, conn, trans))
                        {
                            cmd.Parameters.AddWithValue("@marka", gailua.Marka);
                            cmd.Parameters.AddWithValue("@data", gailua.ErosketaData);
                            cmd.Parameters.AddWithValue("@egoera", gailua.Egoera);
                            cmd.Parameters.AddWithValue("@mintegia", gailua.IdMintegia);
                            cmd.Parameters.AddWithValue("@kokalekua", idKokalekua);
                            cmd.ExecuteNonQuery();
                            idGenerado = (int)cmd.LastInsertedId;
                        }

                        // 2. Txertatu taula espezifikoan
                        if (gailua is Ordenagailua ordenador)
                        {
                            string sqlOrdenador = "INSERT INTO Ordenagailua (gailua_id, prozesagailua, ram) VALUES (@id, @cpu, @ram)";
                            using (var cmd = new MySqlCommand(sqlOrdenador, conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@id", idGenerado);
                                cmd.Parameters.AddWithValue("@cpu", ordenador.Prozesagailua);
                                cmd.Parameters.AddWithValue("@ram", ordenador.RamGB.ToString() + "GB");
                                cmd.ExecuteNonQuery();
                            }
                        }
                        else if (gailua is Inprimagailua impresora)
                        {
                            string sqlImpresora = "INSERT INTO Inprimagailua (gailua_id, koloretakoa) VALUES (@id, @color)";
                            using (var cmd = new MySqlCommand(sqlImpresora, conn, trans))
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

        public List<Gailua> LortuGailuak()
        {
            List<Gailua> zerrenda = new List<Gailua>();
            using (var conn = konexioa.LortuKonexioa())
            {
                conn.Open();
                
                // JOIN con Kokalekua para obtener el nombre real
                string sqlPC = "SELECT g.*, o.prozesagailua, o.ram, k.izena as k_izena FROM Gailua g " +
                              "INNER JOIN Ordenagailua o ON g.id_gailua = o.gailua_id " +
                              "INNER JOIN Kokalekua k ON g.id_kokalekua = k.id_kokalekua";
                using (var cmd = new MySqlCommand(sqlPC, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string ramText = reader["ram"].ToString() ?? "0";
                        int ram = int.TryParse(ramText.Replace("GB", ""), out int r) ? r : 0;
                        zerrenda.Add(new Ordenagailua(
                            Convert.ToInt32(reader["id_gailua"]),
                            reader["marka"].ToString() ?? "",
                            reader["k_izena"].ToString() ?? "",
                            reader["egoera"].ToString() ?? "",
                            Convert.ToDateTime(reader["erosketa_data"]),
                            reader["prozesagailua"].ToString() ?? "",
                            ram
                        ));
                    }
                }

                string sqlPrint = "SELECT g.*, i.koloretakoa, k.izena as k_izena FROM Gailua g " +
                                 "INNER JOIN Inprimagailua i ON g.id_gailua = i.gailua_id " +
                                 "INNER JOIN Kokalekua k ON g.id_kokalekua = k.id_kokalekua";
                using (var cmd = new MySqlCommand(sqlPrint, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        zerrenda.Add(new Inprimagailua(
                            Convert.ToInt32(reader["id_gailua"]),
                            reader["marka"].ToString() ?? "",
                            reader["k_izena"].ToString() ?? "",
                            reader["egoera"].ToString() ?? "",
                            Convert.ToDateTime(reader["erosketa_data"]),
                            Convert.ToBoolean(reader["koloretakoa"])
                        ));
                    }
                }
            }
            return zerrenda;
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
                        // 1. Txertatu historikoan
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