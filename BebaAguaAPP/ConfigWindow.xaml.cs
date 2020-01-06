using BebaAguaAPP.Classes;
using System;
using System.Collections.Generic;
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


namespace BebaAguaAPP
{

  
    public partial class ConfigWindow: Window
    {
        private static SQLiteConnection sqliteConnection;
        public static int lastID;
        public ConfigWindow()
        {
                InitializeComponent();



            sqliteConnection = new SQLiteConnection("Data Source=.\\dados\\DadosAgua.db; Version=3;");
            sqliteConnection.Open();

            SQLiteCommand cmd = new SQLiteCommand("Select * from DadosAgua", sqliteConnection);
            SQLiteDataReader da = cmd.ExecuteReader();
            while (da.Read())
            {
                pegaTotal.Text = da.GetValue(2).ToString();
                pegaCopo.Text = da.GetValue(1).ToString();
            }

            sqliteConnection.Close();

        }

    
        private void DefinAgua(object sender, RoutedEventArgs e)
        {
           
            if ( pegaCopo.Text == "" && pegaTotal.Text == "" )
            {
                MessageBox.Show("Informações Inválidas!!! Preencha os campos primeiro", "BebaAguaAPP", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ( pegaCopo.Text == "" || pegaTotal.Text == "")
            {
                MessageBox.Show("Informações Inválidas!!! Preencha todos os campos", "BebaAguaAPP", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else {
                MainWindow.copoml = int.Parse(pegaCopo.Text);
                MainWindow.totalml = int.Parse(pegaTotal.Text);
                if (MainWindow.copoml > 2000 && MainWindow.totalml > 10000 || MainWindow.copoml > 2000 || MainWindow.totalml > 10000)
                {
                    MessageBox.Show("Informações Inválidas o copo tem que ser até 2 litros e o total até 10 litros!!!", "BebaAguaAPP", MessageBoxButton.OK, MessageBoxImage.Information);
                } else
                {
                    MessageBox.Show("Configurações Atualizadas com sucesso!!", "BebaAguaAPP", MessageBoxButton.OK, MessageBoxImage.Information);

                    try
                    {
                        DadosAgua dad = new DadosAgua();

                       

                        sqliteConnection = new SQLiteConnection("Data Source=.\\dados\\DadosAgua.db; Version=3;");
                        sqliteConnection.Open();

                        SQLiteCommand cmd = new SQLiteCommand("Select * from DadosAgua", sqliteConnection);
                        SQLiteDataReader da = cmd.ExecuteReader();
                        while (da.Read())
                        {
                            lastID = Convert.ToInt32(da.GetValue(0));
                        }

                        dad.Id = lastID;
                        dad.ValorCopo = pegaCopo.Text;
                        dad.ValorTotal = pegaTotal.Text;
                        dad.Contador = MainWindow.contador.ToString();          

                        DalHelper.Update(dad);
                        pegaTotal.Text = "";
                        pegaCopo.Text = "";
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro : " + ex.Message);
                    }

                  Close();
                }
             
            }
        }

        private void CloseApp(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BebaAguaAPP.BlurFeature.NativeBlurBackground.EnableBlur(this);
        }

        private void PegaAgua_KeyDown_1(object sender, KeyEventArgs e)
            {
                e.Handled = !IsNumberKey(e.Key);
            }
            private bool IsNumberKey(Key inKey)
            {
                if (inKey < Key.D0 || inKey > Key.D9)
                {
                    if (inKey < Key.NumPad0 || inKey > Key.NumPad9)
                    {
                        return false;
                    }
                }
                return true;
        }
    }
}