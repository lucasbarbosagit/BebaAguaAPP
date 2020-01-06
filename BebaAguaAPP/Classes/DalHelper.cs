using System;
using System.Data.SQLite;
using BebaAguaAPP.Classes;
using System.Data;

namespace BebaAguaAPP
{
    public class DalHelper
    {
        private static SQLiteConnection sqliteConnection;
        public DalHelper()
        { }
        public static SQLiteConnection DbConnection()
        {
            sqliteConnection = new SQLiteConnection("Data Source=.\\dados\\DadosAgua.db; Version=3;");


            sqliteConnection.Open();
            return sqliteConnection;
        }
        public static void CriarBancoSQLite()
        {
            try
            {
                SQLiteConnection.CreateFile(AppDomain.CurrentDomain.BaseDirectory + @"\dados\DadosAgua.db");
            }
            catch
            {
                throw;
            }
        }
        public static void CriarTabelaSQlite()
        {
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS DadosAgua(id int, ValorCopo Varchar(50), ValorTotal VarChar(80), Contador VarChar(50), carinha VarChar(20))";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Add(DadosAgua dadosAgua)
        {
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO DadosAgua(id, ValorCopo, ValorTotal, Contador, carinha, Data, Hora) values (@id, @Valorcopo, @ValorTotal, @Contador, @carinha, @Data, @Hora)";
                    cmd.Parameters.AddWithValue("@Id", dadosAgua.Id);
                    cmd.Parameters.AddWithValue("@ValorCopo", dadosAgua.ValorCopo);
                    cmd.Parameters.AddWithValue("@ValorTotal", dadosAgua.ValorTotal);
                    cmd.Parameters.AddWithValue("@Contador", dadosAgua.Contador);
                    cmd.Parameters.AddWithValue("@carinha", dadosAgua.carinha);
                    cmd.Parameters.AddWithValue("@Data", dadosAgua.Data);
                    cmd.Parameters.AddWithValue("@Hora", dadosAgua.Hora);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Update(DadosAgua dadosAgua)
        {
            try
            {
                using (var cmd = new SQLiteCommand(DbConnection()))
                {
                    if (dadosAgua.Id != null)
                    {
                        cmd.CommandText = "UPDATE DadosAgua SET Contador=@Contador, ValorCopo=@ValorCopo, ValorTotal=@ValorTotal WHERE id = @id";
                        cmd.Parameters.AddWithValue("@Id", dadosAgua.Id);
                        cmd.Parameters.AddWithValue("@Contador", dadosAgua.Contador);
                        cmd.Parameters.AddWithValue("@ValorCopo", dadosAgua.ValorCopo);
                        cmd.Parameters.AddWithValue("@ValorTotal", dadosAgua.ValorTotal);
                      
                        cmd.ExecuteNonQuery();
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Update2(DadosAgua dadosAgua)
        {
            try
            {
                using (var cmd = new SQLiteCommand(DbConnection()))
                {
                    if (dadosAgua.Id != null)
                    {
                        cmd.CommandText = "UPDATE DadosAgua SET Contador=@Contador WHERE Id=@Id";
                        cmd.Parameters.AddWithValue("@Id", dadosAgua.Id);
                        cmd.Parameters.AddWithValue("@Contador", dadosAgua.Contador);
                        
                        cmd.ExecuteNonQuery();
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Update3(DadosAgua dadosAgua)
        {
            try
            {
                using (var cmd = new SQLiteCommand(DbConnection()))
                {
                    if (dadosAgua.Id != null)
                    {
                        cmd.CommandText = "UPDATE DadosAgua SET carinha=@carinha WHERE Id=@Id";
                        cmd.Parameters.AddWithValue("@Id", dadosAgua.Id);
                        cmd.Parameters.AddWithValue("@carinha", dadosAgua.carinha);

                        cmd.ExecuteNonQuery();
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       

        public static DataTable GetDadosAguas()
        {
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM DadosAgua";
                    da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DataTable GetDadosAgua(int id)
        {
            SQLiteDataAdapter da = null;
            DataTable dt = new DataTable();
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM DadosAgua Where Id=" + id;
                    da = new SQLiteDataAdapter(cmd.CommandText, DbConnection());
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static void Delete(int Id)
        {
            try
            {
                using (var cmd = new SQLiteCommand(DbConnection()))
                {
                    cmd.CommandText = "DELETE FROM DadosAgua Where Id=@Id";
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
