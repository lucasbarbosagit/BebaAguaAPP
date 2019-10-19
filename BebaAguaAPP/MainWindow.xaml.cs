using SQLite;
using BebaAguaAPP.Classes;
using System;
using System.Windows;
using System.Xml;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using System.IO;
using System.Windows.Threading;
using System.Windows.Input;

namespace BebaAguaAPP
{

    public partial class MainWindow : Window
    {
        DadosAgua dadosaguinha;
        int PADDING = 5;
        public static int Pegaaguavalor = 0;
        public static int contador = 0;
        public static int totalml = 0;
        public static int copoml = 0;
        private const String APP_ID = "BebaAguaAPP";
     
        public MainWindow()
        {
            InitializeComponent();
            dadosaguinha = new DadosAgua();
            
            this.ShowInTaskbar = true;
            this.Left = SystemParameters.PrimaryScreenWidth - this.Width - PADDING;
            System.Windows.Threading.DispatcherTimer DispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            DispatcherTimer.Tick += DispatcherTimer_Tick;
            DispatcherTimer.Interval = new TimeSpan(0, 0, 3600);
            DispatcherTimer.Start();
          //  ReadDatabase();
           
        }
   
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            // Get a toast XML template
            Windows.Data.Xml.Dom.XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText03);

            // Fill in the text elements
            Windows.Data.Xml.Dom.XmlNodeList stringElements = toastXml.GetElementsByTagName("text");
            for (int i = 0; i < stringElements.Length; i++)
            {
                stringElements[i].AppendChild(toastXml.CreateTextNode("Line " + i));
            }

            // Specify the absolute path to an image
            String imagePath = "file:///" + Path.GetFullPath(@"/bebaagua.jpg");
            Windows.Data.Xml.Dom.XmlNodeList imageElements = toastXml.GetElementsByTagName("image");
            imageElements[0].Attributes.GetNamedItem("src").NodeValue = imagePath;

            // Create the toast and attach event listeners
            ToastNotification toast = new ToastNotification(toastXml);

            // Show the toast. Be sure to specify the AppUserModelId on your application's shortcut!
            ToastNotificationManager.CreateToastNotifier(APP_ID).Show(toast);
        }

        private void CloseApp(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BebaAguaAPP.BlurFeature.NativeBlurBackground.EnableBlur(this);
        }

       // void ReadDatabase()
      //  {
        //    using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.databasePath))
      //     {
          //      conn.CreateTable<DadosAgua>();
           //     contador = int.Parse(dadosaguinha.Contador);
        //    }

           
       // }

        public void PegarAgua(object sender, RoutedEventArgs e)
        {
           
               
          
              if (pegaAgua.Text == "" || copoml == 0 && totalml == 0)
               {
                  MessageBox.Show("Defina as Configurações Primeiro!!!", "BebaAguaAPP", MessageBoxButton.OK, MessageBoxImage.Information);
               }
               else
               {


               



                Pegaaguavalor = int.Parse(this.pegaAgua.Text) + Pegaaguavalor;
                contador = Pegaaguavalor * copoml;
                dadosaguinha.Contador = contador.ToString();
                textContador.Text = dadosaguinha.Contador;
                pegaAgua.Text = "";

                using (SQLiteConnection con = new SQLiteConnection(App.databasePath))
                {

                    con.CreateTable<DadosAgua>();
                    con.Query<DadosAgua>("UPDATE DADOSAGUA set Contador=? Where Id=1", dadosaguinha.Contador);
                   
                    

                }

                if (contador < totalml / 2)
                {
                    // this.textStatus.Text = "Ainda Falta!!!";
                    //  this.textStatus.FontSize = 24;
                    //    this.textStatus.Width = 300;
                    haha.Visibility = Visibility.Collapsed;
                    triste.Visibility = Visibility.Visible;
                    wow.Visibility = Visibility.Collapsed;
                    zen.Visibility = Visibility.Collapsed;
                    // Thickness margin = textContador.Margin;
                    //   margin.Left = 74;
                    //  textContador.Margin = margin;

                }

                if (contador > totalml / 2 && contador < totalml)
                {
                    // this.textStatus.Text = "Passou da Metade, Quase lá!!!";
                    //   this.textStatus.FontSize = 24;
                    // this.textStatus.Width = 300;
                    zen.Visibility = Visibility.Visible;
                    haha.Visibility = Visibility.Collapsed;
                    triste.Visibility = Visibility.Collapsed;
                    wow.Visibility = Visibility.Collapsed;
                    // Thickness margin = textContador.Margin;
                    //   margin.Left = 74;
                    //  textContador.Margin = margin;

                }

                if (contador == totalml)
                {
                    //  this.textStatus.Text = "Parabéns!!!!";
                    //  this.textStatus.FontSize = 24;
                    haha.Visibility = Visibility.Visible;
                    wow.Visibility = Visibility.Collapsed;
                    triste.Visibility = Visibility.Collapsed;
                    zen.Visibility = Visibility.Collapsed;
                }

                //  Limitar contador
                if (contador > totalml)
                {
                    // this.textStatus.Text = "Você atingiu o limite.";
                    // this.textStatus.FontSize = 24;
                    //  this.textStatus.Width = 300;
                    //  contador = 99;
                    // Pegaaguavalor = 99;
                    textContador.Text = contador.ToString();
                    wow.Visibility = Visibility.Visible;
                    haha.Visibility = Visibility.Collapsed;
                    triste.Visibility = Visibility.Collapsed;
                    zen.Visibility = Visibility.Collapsed;
                }

                if (contador >= 10000)
                {
                    // this.textStatus.Text = "Você atingiu o limite.";
                    // this.textStatus.FontSize = 24;
                    //  this.textStatus.Width = 300;
                    contador = 10000;
                    // Pegaaguavalor = 99;
                    textContador.Text = contador.ToString();
                    wow.Visibility = Visibility.Visible;
                    haha.Visibility = Visibility.Collapsed;
                    triste.Visibility = Visibility.Collapsed;
                    zen.Visibility = Visibility.Collapsed;
                }
            }

        }
        private void ResetarAgua(object sender, RoutedEventArgs e)
        {

          
            if (pegaAgua.Text == "" && copoml == 0 && totalml == 0 || pegaAgua.Text != "" && copoml == 0 && totalml == 0 || pegaAgua.Text != "" 
                && copoml != 0 && totalml != 0 && contador == 0)
            {
                MessageBox.Show("Defina as Configurações Primeiro!!!", "BebaAguaAPP", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else {

               

                contador = 0;
            Pegaaguavalor = 0;
            textContador.Text = contador.ToString();
            pegaAgua.Text = "";  
        //    textStatus.Text = "Ainda Falta!!!!";
            haha.Visibility = Visibility.Collapsed;
            wow.Visibility = Visibility.Collapsed;
            triste.Visibility = Visibility.Visible;
            zen.Visibility = Visibility.Collapsed;
            }
        }

        private void PegaAgua_KeyDown_1(object sender, System.Windows.Input.KeyEventArgs e)
     
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

        private void ConfigWindowAbrir(object sender, RoutedEventArgs e)
        {
            ConfigWindow w2 = new ConfigWindow();
            w2.ShowDialog();
        }

        private void MinimizeApp(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}