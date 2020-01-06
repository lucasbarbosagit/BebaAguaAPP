using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SQLite;
using BebaAguaAPP.Classes;


namespace BebaAguaAPP
{
    /// <summary>
    /// Lógica interna para HistDadosAgua.xaml
    /// </summary>
    public partial class HistDadosAgua : Window
    {
        public HistDadosAgua()
        {
            InitializeComponent();


            DataTable dt = new DataTable();

            String insSQL = "select * from DadosAgua";
            String strConn = @"Data Source=.\\dados\\DadosAgua.db;";

            SQLiteConnection conn = new SQLiteConnection(strConn);

            SQLiteDataAdapter da = new SQLiteDataAdapter(insSQL, strConn);
           

            da.Fill(dt);
            dvgDadosAgua.ItemsSource = dt.DefaultView;
        }

        private void LocalizarDados(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                MessageBox.Show("Informe o ID da entrada a ser Localizada");
                return;
            }
            try
            {
                DataTable dt = new DataTable();
                int codigo = Convert.ToInt32(txtID.Text);

                dt = DalHelper.GetDadosAgua(codigo);
                dvgDadosAgua.ItemsSource = dt.DefaultView;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
        }
        private static SQLiteConnection sqliteConnection;
        public void BtnExcluirDados(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                MessageBox.Show("Informe o ID da entrada a ser Excluída");
                return;
            }

            try
            {
                int codigo = Convert.ToInt32(txtID.Text);
                DalHelper.Delete(codigo);
                ExibirDados();
                LimpaDados();

                
          
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
        }

        private void LimpaDados()
        {
            txtID.Text = "";
            txtContador.Text = "";
            txtValorCopo.Text = "";
            txtValorTotal.Text = "";
        }


        private void BtnAtualizarDados(object sender, RoutedEventArgs e)
        {
            try
            {
                DadosAgua dad = new DadosAgua();
                dad.Id = Convert.ToInt32(txtID.Text);
                dad.ValorCopo = txtValorCopo.Text;
                dad.ValorTotal = txtValorTotal.Text;
                dad.Contador = txtContador.Text;

                DalHelper.Update(dad);
                ExibirDados();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }
        }

        private void ExibirDados()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = DalHelper.GetDadosAguas();
                dvgDadosAgua.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }


        }

        private void Window_Closed(object sender, EventArgs e)
        {
            sqliteConnection = new SQLiteConnection("Data Source=.\\dados\\DadosAgua.db; Version=3;");
            sqliteConnection.Open();

            SQLiteCommand cmd = new SQLiteCommand("Select * from DadosAgua", sqliteConnection);
            SQLiteDataReader da = cmd.ExecuteReader();
            while (da.Read())
            {
               // textContador.Text = da.GetValue(3).ToString();
                MainWindow.contador = Convert.ToInt32(da.GetValue(3));
                MainWindow.copoml = Convert.ToInt32(da.GetValue(1));
                MainWindow.totalml = Convert.ToInt32(da.GetValue(2));
              //  MainWindow.Pegaaguavalor = Convert.ToInt32(da.GetValue(4));
            }

            sqliteConnection.Close();
        }

        private void MostrarBebidoDia_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sqliteConnection = new SQLiteConnection("Data Source=.\\dados\\DadosAgua.db; Version=3;");
                sqliteConnection.Open();

                string datepicker1 = datee.Text;
                string sqlquery = ("SELECT SUM(ValorCopo) FROM DadosAgua WHERE Data = '" + datepicker1 + "'");
                SQLiteCommand cmd = new SQLiteCommand(sqlquery, sqliteConnection);

                txtBebidoDia.Text = cmd.ExecuteScalar().ToString() + " ML";
                sqliteConnection.Close();

                if (txtBebidoDia.Text == " ML")
                {
                    txtBebidoDia.Text = "Nada Bebido";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MostrarBebidoMes_Click(object sender, RoutedEventArgs e)
        {
               try
                {
                    sqliteConnection = new SQLiteConnection("Data Source=.\\dados\\DadosAgua.db; Version=3;");
                    sqliteConnection.Open();

                    string datepicker1 = dateMes.Text;
                    string sqlquery = ("SELECT SUM(ValorCopo) FROM DadosAgua WHERE data LIKE '%" + datepicker1 + "%'");
                    SQLiteCommand cmd = new SQLiteCommand(sqlquery, sqliteConnection);

                    txtBebidoMes.Text = cmd.ExecuteScalar().ToString() + " ML";

                    sqliteConnection.Close();
                if (txtBebidoMes.Text == " ML")
                {
                    txtBebidoMes.Text = "Nada Bebido";
                }

                  
                }
                catch (Exception ex)
                {
                    throw ex;
                }  

        }

        private void DvgDadosAgua_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if(row_selected != null)
            {
                txtID.Text = row_selected["Id"].ToString();
                txtContador.Text = row_selected["Contador"].ToString();
                txtValorCopo.Text = row_selected["ValorCopo"].ToString();
                txtValorTotal.Text = row_selected["ValorTotal"].ToString();
            }
        }

        //     private void DataGrid_AutoGeneratedColunns(object sender, EventArgs e)
        //  {
        //     if (dvgDadosAgua.Columns.Count > 0)
        //    {
        //      dvgDadosAgua.Columns.Where(a => a.Header.ToString() == "PegaAguaValor").First().Header = "Copos Bebidos";
        //   }
        //  }


    }
}