using BebaAguaAPP.Classes;
using SQLite;
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


namespace BebaAguaAPP
{

    /// <summary>
    /// Lógica interna para Window1.xaml
    /// </summary>
    public partial class ConfigWindow: Window
    {
        DadosAgua dadosaguinha;
        public ConfigWindow()
        {
                InitializeComponent();

            dadosaguinha = new DadosAgua();
               
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
                    DadosAgua dadosAgua = new DadosAgua()
                    {
                        ValorCopo = pegaCopo.Text,
                        ValorTotal = pegaTotal.Text,
                        Contador = "0"
                    };
                
                  

                    using (SQLiteConnection connection = new SQLiteConnection(App.databasePath))
                    {
                        connection.CreateTable<DadosAgua>();
                        connection.Insert(dadosAgua);
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