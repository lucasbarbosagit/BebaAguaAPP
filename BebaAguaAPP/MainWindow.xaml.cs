using BebaAguaAPP.Classes;
using System;
using System.Windows;
using System.Xml;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using System.IO;
using System.Windows.Threading;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Data.SQLite;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Media;


namespace BebaAguaAPP
{

    public partial class MainWindow : Window
    {
        int PADDING = 5; 
        public static int contador = 0;
        public static int totalml = 0;
        public static int copoml = 0;
        public static int lastID;
        private const String APP_ID = "BebaAguaAPP";
        private static SQLiteConnection sqliteConnection;
        public MainWindow()
        {
            InitializeComponent();
            MostrarSplashScreen();

            sqliteConnection = new SQLiteConnection("Data Source=.\\dados\\DadosAgua.db; Version=3;");
            sqliteConnection.Open();

            SQLiteCommand cmd = new SQLiteCommand("Select * from DadosAgua", sqliteConnection);
            SQLiteDataReader da = cmd.ExecuteReader();
            while (da.Read())
            {
                textContador.Text = da.GetValue(3).ToString();
                contador = Convert.ToInt32(da.GetValue(3));
                copoml = Convert.ToInt32(da.GetValue(1));
                totalml = Convert.ToInt32(da.GetValue(2));
                carinha.Source = (ImageSource)Resources[da.GetValue(4)];         
            }

            sqliteConnection.Close();


            if (copoml == 0 && totalml == 0)
            {

                MessageBox.Show("Defina as Configurações Primeiro!!!", "BebaAguaAPP", MessageBoxButton.OK, MessageBoxImage.Information);
                ConfigWindow w2 = new ConfigWindow();
                w2.ShowDialog();
              

            }

            this.ShowInTaskbar = true;
            this.Left = SystemParameters.PrimaryScreenWidth - this.Width - PADDING;
            System.Windows.Threading.DispatcherTimer DispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            DispatcherTimer.Tick += DispatcherTimer_Tick;
            DispatcherTimer.Interval = new TimeSpan(0, 0, 3600);
            DispatcherTimer.Start();

        }


       
      
        public void MostrarSplashScreen()
        {
            try
            {
                SplashScreen splashScreen = new SplashScreen("logopcj12.jpg");
                splashScreen.Show(false);
                Thread.Sleep(2000);
                splashScreen.Close(new TimeSpan(0, 0, 0));

            }
            catch (Exception ex1)
            {
                MessageBox.Show("Erro" + ex1.Message);
            }
        }



        public static string mensagem1 = "Mantenha-se Hidratado, beba água !!!";
        public static string mensagem2 = "Você ainda está no início, continue bebendo mais água!";
        public static string mensagem3 = "Boa, você chegou na metade, continue bebendo mais água!!!";
        public static string mensagem4 = "Você passou da metade, Parabéns!!!";
        public static string mensagem5 = "Parabéns você chegou no seu limite diário!!!";
        public static string mensagem6 = "Você passou do dobro do seu limite diário!!!";


        public void Notifica(string mensagem1)
        {


            Windows.Data.Xml.Dom.XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText03);

            // Fill in the text elements
            Windows.Data.Xml.Dom.XmlNodeList stringElements = toastXml.GetElementsByTagName("text");
            stringElements[0].AppendChild(toastXml.CreateTextNode("Beba Água APP"));
            stringElements[1].AppendChild(toastXml.CreateTextNode(mensagem1));



            // Specify the absolute path to an image
            String imagePath = System.IO.Path.GetDirectoryName(
    System.Reflection.Assembly.GetExecutingAssembly().Location);
            Windows.Data.Xml.Dom.XmlNodeList imageElements = toastXml.GetElementsByTagName("image");
            imageElements[0].Attributes.GetNamedItem("src").NodeValue = imagePath;

            // Create the toast and attach event listeners
            ToastNotification toast = new ToastNotification(toastXml);

            // Show the toast. Be sure to specify the AppUserModelId on your application's shortcut!
            ToastNotificationManager.CreateToastNotifier(APP_ID).Show(toast);
        }
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            Notifica(mensagem1);
        }


        private void CloseApp(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BebaAguaAPP.BlurFeature.NativeBlurBackground.EnableBlur(this);
        }


        public static String GetData(DateTime value)
        {
            return value.ToString("dd/MM/yyyy");
        }
        //  ...later on in the code
        String getData = GetData(DateTime.Now);

        public static String GetHora(DateTime value)
        {
            return value.ToString("hh:mm");
        }
        //  ...later on in the code
        String getHora = GetHora(DateTime.Now);



        public void PegarAgua(object sender, RoutedEventArgs e)
        {
            //if (pegaAgua.Text == "" ||  copoml == 0 && totalml == 0)
          //  {
           //     MessageBox.Show("Defina as Configurações Primeiro!!!", "BebaAguaAPP", MessageBoxButton.OK, MessageBoxImage.Information);
          //  }
          //  else
            //  { 
               // Pegaaguavalor = int.Parse(this.pegaAgua.Text) + Pegaaguavalor;
                contador = contador + copoml;
                textContador.Text = contador.ToString();           
            
                try
                {
                    DadosAgua dad = new DadosAgua();
                   
                    dad.Contador = contador.ToString();
                    dad.ValorCopo = copoml.ToString();
                    dad.ValorTotal = totalml.ToString();
                    dad.carinha = "triste";
                    dad.Data = getData;
                    dad.Hora = getHora;

                DalHelper.Add(dad);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro : " + ex.Message);
                }
            
               if (contador < totalml / 2)
                {
                     
                    carinha.Source = (ImageSource) Resources["triste"];
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
                    dad.carinha = "triste";
                   

                    DalHelper.Update3(dad);
                    Notifica(mensagem2);


                }

                if (contador == totalml / 2)
                {
                    carinha.Source = (ImageSource) Resources["zen1"];

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
                     dad.carinha = "zen1";

                    DalHelper.Update3(dad);
                    Notifica(mensagem3);

                }

                if (contador > totalml / 2 && contador < totalml)
                {

                carinha.Source = (ImageSource)Resources["zen2"];

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
                dad.carinha = "zen2";

                DalHelper.Update3(dad);

                Notifica(mensagem4);

                }

                if (contador >= totalml && contador < totalml * 2)
                {
                carinha.Source = (ImageSource)Resources["haha"];

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
                dad.carinha = "haha";

                DalHelper.Update3(dad);
                Notifica(mensagem5);

                }

              
                if (contador >= totalml * 2)
                {

                carinha.Source = (ImageSource)Resources["wow"];

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
                dad.carinha = "wow";

                DalHelper.Update3(dad);
                Notifica(mensagem6);
                }

                if (contador >= 10000)
                {                 
                    contador = 10000;

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
                        dad.Contador = "10000";
                      
                      
                        DalHelper.Update2(dad);

                        dad.carinha = "wow";
                        DalHelper.Update3(dad);
                }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro : " + ex.Message);
                    }
                    
                    textContador.Text = "10000";
                    carinha.Source = (ImageSource)Resources["wow"];

            }
        }
        private void ResetarAgua(object sender, RoutedEventArgs e)
        {

          //  if (pegaAgua.Text == "" && copoml == 0 && totalml == 0 || pegaAgua.Text != "" && copoml == 0 && totalml == 0 || pegaAgua.Text != "" 
              //  && copoml != 0 && totalml != 0 && contador == 0)
         //   {
               // MessageBox.Show("Defina as Configurações Primeiro!!!", "BebaAguaAPP", MessageBoxButton.OK, MessageBoxImage.Information);
         //   }
           // else {
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
                    dad.Contador = "0";
                    dad.PegaAguaValor = "0";

                    DalHelper.Update2(dad);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro : " + ex.Message);
                }


            contador = 0;
            textContador.Text = "0";
            carinha.Source = (ImageSource)Resources["triste"];

        }

        private void PegaAgua_KeyDown_1(object sender, System.Windows.Input.KeyEventArgs e)
     
        {
            e.Handled = !IsNumberKey(e.Key);
            if (e.Key == Key.Enter)
            {
                PegarAgua(sender, e);
            }
           
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

        private void Window_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
                if (e.ChangedButton == MouseButton.Left)
                    this.DragMove();     
        }

        private void HistAbrir(object sender, RoutedEventArgs e)
        {
            HistDadosAgua w3 = new HistDadosAgua();
            w3.ShowDialog();
        }

        private void SobreAbrir(object sender, RoutedEventArgs e)
        {
            Sobre w4 = new Sobre();
            w4.ShowDialog();
        }

        private void ReturnAgua(object sender, RoutedEventArgs e)
        {
           

            sqliteConnection = new SQLiteConnection("Data Source=.\\dados\\DadosAgua.db; Version=3;");
            sqliteConnection.Open();

            SQLiteCommand cmd = new SQLiteCommand("Select * from DadosAgua", sqliteConnection);
            SQLiteDataReader da = cmd.ExecuteReader();
            while (da.Read())
            {
                lastID = Convert.ToInt32(da.GetValue(0));
                textContador.Text = da.GetValue(3).ToString();
            }

            sqliteConnection.Close();

            DalHelper.Delete(lastID);

            sqliteConnection = new SQLiteConnection("Data Source=.\\dados\\DadosAgua.db; Version=3;");
            sqliteConnection.Open();

            SQLiteCommand cmd2 = new SQLiteCommand("Select * from DadosAgua", sqliteConnection);
            SQLiteDataReader da2 = cmd2.ExecuteReader();
            while (da2.Read())
            {
                textContador.Text = da2.GetValue(3).ToString();
            }

            sqliteConnection.Close();

            //falta o metodo atualizadados() separado

            MessageBox.Show("Entrada Excluída com Sucesso!!!", "BebaAguaAPP", MessageBoxButton.OK, MessageBoxImage.Information);

        }

  

        private void Window_MouseLeave_1(object sender, MouseEventArgs e)
        {
            Thread.Sleep(500);
            this.Width = 250;
            this.Height = 110;
            this.WindowStyle = WindowStyle.None;
            textContador.HorizontalAlignment = HorizontalAlignment.Center;
            textCont.HorizontalAlignment = HorizontalAlignment.Center;
            voltarDadosAgua.Visibility = Visibility.Collapsed;
            vbox.Visibility = Visibility.Collapsed;

        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            Thread.Sleep(500);
            this.Width = 444.207; 
            this.Height = 199.175;
            this.WindowStyle = WindowStyle.None;
            textContador.HorizontalAlignment = HorizontalAlignment.Right;
            textML.HorizontalAlignment = HorizontalAlignment.Right;
            textCont.HorizontalAlignment = HorizontalAlignment.Right;
            voltarDadosAgua.Visibility = Visibility.Visible;
            vbox.Visibility = Visibility.Visible;
            

        }

        private void ThumbButtonInfo_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Clicked");
        }

        private void ThumbButtonInfo_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Clicked");
        }
    }
}