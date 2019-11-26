using System;
using System.Data.SQLite;
using BebaAguaAPP.Classes;
namespace BebaAguaAPP
{
    public class DalHelper
    {
        private static SQLiteConnection sqliteConnection;
        public DalHelper()
        { }
         public static SQLiteConnection DbConnection()
        {
            sqliteConnection = new SQLiteConnection("Data Source=c:\\Users\\Lucas\\Documents\\DadosAgua.db; Version=3;");
            sqliteConnection.Open(); 
            return sqliteConnection;
        }
        public static void CriarBancoSQLite()
        {
            try
            {
                SQLiteConnection.CreateFile(@"c:\Users\Lucas\Documents\DadosAgua.db");
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
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS DadosAgua(id int, ValorCopo Varchar(50), ValorTotal VarChar(80), Contador VarChar(50), PegaAguaValor VarChar(50))";
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
                    cmd.CommandText = "INSERT INTO DadosAgua(id, ValorCopo, ValorTotal, Contador, PegaAguaValor) values (@id, @Valorcopo, @ValorTotal, @Contador, @PegaAguaValor)";
                    cmd.Parameters.AddWithValue("@Id", dadosAgua.Id);
                    cmd.Parameters.AddWithValue("@ValorCopo", dadosAgua.ValorCopo);
                    cmd.Parameters.AddWithValue("@ValorTotal", dadosAgua.ValorTotal);
                    cmd.Parameters.AddWithValue("@Contador", dadosAgua.Contador);
                    cmd.Parameters.AddWithValue("@PegaAguaValor", dadosAgua.PegaAguaValor);
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
                        cmd.CommandText = "UPDATE DadosAgua SET Contador=@Contador, ValorCopo=@ValorCopo, ValorTotal=@ValorTotal, PegaAguaValor=@PegaAguaValor WHERE Id=@Id";
                        cmd.Parameters.AddWithValue("@Id", dadosAgua.Id);           
                        cmd.Parameters.AddWithValue("@Contador", dadosAgua.Contador);
                        cmd.Parameters.AddWithValue("@ValorCopo", dadosAgua.ValorCopo);
                        cmd.Parameters.AddWithValue("@ValorTotal", dadosAgua.ValorTotal);
                        cmd.Parameters.AddWithValue("@PegaAguaValor", dadosAgua.PegaAguaValor);
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
                        cmd.CommandText = "UPDATE DadosAgua SET Contador=@Contador, PegaAguaValor=@PegaAguaValor WHERE Id=@Id";
                        cmd.Parameters.AddWithValue("@Id", dadosAgua.Id);
                        cmd.Parameters.AddWithValue("@Contador", dadosAgua.Contador);
                        cmd.Parameters.AddWithValue("@PegaAguaValor", dadosAgua.PegaAguaValor);
                        cmd.ExecuteNonQuery();
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
