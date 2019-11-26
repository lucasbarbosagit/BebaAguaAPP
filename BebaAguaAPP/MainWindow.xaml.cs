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

namespace BebaAguaAPP
{

    public partial class MainWindow : Window
    {
        int PADDING = 5; 
        public static int Pegaaguavalor = 3;
        public static int contador = 0;
        public static int totalml = 0;
        public static int copoml = 0;
        private const String APP_ID = "BebaAguaAPP";
        private static SQLiteConnection sqliteConnection;
        public MainWindow()
        {
            InitializeComponent();

            sqliteConnection = new SQLiteConnection("Data Source=c:\\Users\\Lucas\\Documents\\DadosAgua.db; Version=3;");
            sqliteConnection.Open();

            SQLiteCommand cmd = new SQLiteCommand("Select * from DadosAgua where Id = 2", sqliteConnection);
            SQLiteDataReader da = cmd.ExecuteReader();
            while (da.Read())
            {
                textContador.Text = da.GetValue(3).ToString();
                contador = Convert.ToInt32(da.GetValue(3));
                copoml = Convert.ToInt32(da.GetValue(1));
                totalml = Convert.ToInt32(da.GetValue(2));
                Pegaaguavalor = Convert.ToInt32(da.GetValue(4));
            }

            sqliteConnection.Close();

            this.ShowInTaskbar = true;
            this.Left = SystemParameters.PrimaryScreenWidth - this.Width - PADDING;
            System.Windows.Threading.DispatcherTimer DispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            DispatcherTimer.Tick += DispatcherTimer_Tick;
            DispatcherTimer.Interval = new TimeSpan(0, 0, 3600);
            DispatcherTimer.Start();

        }

        public static string mensagem1 = "Mantenha-se Hidratado, beba água !!!";
        public static string mensagem2 = "Você ainda está no início, continue bebendo mais água!";
        public static string mensagem3 = "Boa, você chegou na metade, continue bebendo mais água!!!";
        public static string mensagem4 = "Você passou da metade, Parabéns!!!";
        public static string mensagem5 = "Parabéns você chegou no seu limite diário!!!";
        public static string mensagem6 = "Você passou do dobro do seu limite diário!!!";

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {

            // Get a toast XML template
            Windows.Data.Xml.Dom.XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText03);

            // Fill in the text elements
            Windows.Data.Xml.Dom.XmlNodeList stringElements = toastXml.GetElementsByTagName("text");
            stringElements[0].AppendChild(toastXml.CreateTextNode("Beba Água APP"));
            stringElements[1].AppendChild(toastXml.CreateTextNode(mensagem1));



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

        public void PegarAgua(object sender, RoutedEventArgs e)
        {
            if (pegaAgua.Text == "" ||  copoml == 0 && totalml == 0)
            {
                MessageBox.Show("Defina as Configurações Primeiro!!!", "BebaAguaAPP", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
              { 
                Pegaaguavalor = int.Parse(this.pegaAgua.Text) + Pegaaguavalor;
                contador = Pegaaguavalor * copoml;
                textContador.Text = contador.ToString();           
                pegaAgua.Text = "";


                try
                {
                    DadosAgua dad = new DadosAgua();
                    dad.Id = Convert.ToInt32(2);
                    dad.Contador = contador.ToString();
                    dad.PegaAguaValor = Pegaaguavalor.ToString();

                    DalHelper.Update2(dad);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro : " + ex.Message);
                }
            
            if (contador < totalml / 2)
                {
                   
                    haha.Visibility = Visibility.Collapsed;
                    triste.Visibility = Visibility.Visible;
                    wow.Visibility = Visibility.Collapsed;
                    zen1.Visibility = Visibility.Collapsed;
                    zen2.Visibility = Visibility.Collapsed;

                    // Get a toast XML template
                    Windows.Data.Xml.Dom.XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText03);

                    // Fill in the text elements
                    Windows.Data.Xml.Dom.XmlNodeList stringElements = toastXml.GetElementsByTagName("text");
                    stringElements[0].AppendChild(toastXml.CreateTextNode("Beba Água APP"));
                    stringElements[1].AppendChild(toastXml.CreateTextNode(mensagem2));



                    // Specify the absolute path to an image
                    String imagePath = "file:///" + Path.GetFullPath(@"/bebaagua.jpg");
                    Windows.Data.Xml.Dom.XmlNodeList imageElements = toastXml.GetElementsByTagName("image");
                    imageElements[0].Attributes.GetNamedItem("src").NodeValue = imagePath;

                    // Create the toast and attach event listeners
                    ToastNotification toast = new ToastNotification(toastXml);

                    // Show the toast. Be sure to specify the AppUserModelId on your application's shortcut!
                    ToastNotificationManager.CreateToastNotifier(APP_ID).Show(toast);

                }

                if (contador == totalml / 2)
                {                   
                    zen1.Visibility = Visibility.Visible;
                    zen2.Visibility = Visibility.Collapsed;
                    haha.Visibility = Visibility.Collapsed;
                    triste.Visibility = Visibility.Collapsed;
                    wow.Visibility = Visibility.Collapsed;

                    // Get a toast XML template
                    Windows.Data.Xml.Dom.XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText03);

                    // Fill in the text elements
                    Windows.Data.Xml.Dom.XmlNodeList stringElements = toastXml.GetElementsByTagName("text");
                    stringElements[0].AppendChild(toastXml.CreateTextNode("Beba Água APP"));
                    stringElements[1].AppendChild(toastXml.CreateTextNode(mensagem3));



                    // Specify the absolute path to an image
                    String imagePath = "file:///" + Path.GetFullPath(@"/bebaagua.jpg");
                    Windows.Data.Xml.Dom.XmlNodeList imageElements = toastXml.GetElementsByTagName("image");
                    imageElements[0].Attributes.GetNamedItem("src").NodeValue = imagePath;

                    // Create the toast and attach event listeners
                    ToastNotification toast = new ToastNotification(toastXml);

                    // Show the toast. Be sure to specify the AppUserModelId on your application's shortcut!
                    ToastNotificationManager.CreateToastNotifier(APP_ID).Show(toast);

                }

                if (contador > totalml / 2 && contador < totalml)
                {
                   
                    zen1.Visibility = Visibility.Collapsed;
                    zen2.Visibility = Visibility.Visible;
                    haha.Visibility = Visibility.Collapsed;
                    triste.Visibility = Visibility.Collapsed;
                    wow.Visibility = Visibility.Collapsed;

                    // Get a toast XML template
                    Windows.Data.Xml.Dom.XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText03);

                    // Fill in the text elements
                    Windows.Data.Xml.Dom.XmlNodeList stringElements = toastXml.GetElementsByTagName("text");
                    stringElements[0].AppendChild(toastXml.CreateTextNode("Beba Água APP"));
                    stringElements[1].AppendChild(toastXml.CreateTextNode(mensagem4));



                    // Specify the absolute path to an image
                    String imagePath = "file:///" + Path.GetFullPath(@"/bebaagua.jpg");
                    Windows.Data.Xml.Dom.XmlNodeList imageElements = toastXml.GetElementsByTagName("image");
                    imageElements[0].Attributes.GetNamedItem("src").NodeValue = imagePath;

                    // Create the toast and attach event listeners
                    ToastNotification toast = new ToastNotification(toastXml);

                    // Show the toast. Be sure to specify the AppUserModelId on your application's shortcut!
                    ToastNotificationManager.CreateToastNotifier(APP_ID).Show(toast);

                }

                if (contador >= totalml && contador < totalml * 2)
                {                  
                    haha.Visibility = Visibility.Visible;
                    wow.Visibility = Visibility.Collapsed;
                    triste.Visibility = Visibility.Collapsed;
                    zen1.Visibility = Visibility.Collapsed;
                    zen2.Visibility = Visibility.Collapsed;

                    // Get a toast XML template
                    Windows.Data.Xml.Dom.XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText03);

                    // Fill in the text elements
                    Windows.Data.Xml.Dom.XmlNodeList stringElements = toastXml.GetElementsByTagName("text");
                    stringElements[0].AppendChild(toastXml.CreateTextNode("Beba Água APP"));
                    stringElements[1].AppendChild(toastXml.CreateTextNode(mensagem5));



                    // Specify the absolute path to an image
                    String imagePath = "file:///" + Path.GetFullPath(@"/bebaagua.jpg");
                    Windows.Data.Xml.Dom.XmlNodeList imageElements = toastXml.GetElementsByTagName("image");
                    imageElements[0].Attributes.GetNamedItem("src").NodeValue = imagePath;

                    // Create the toast and attach event listeners
                    ToastNotification toast = new ToastNotification(toastXml);

                    // Show the toast. Be sure to specify the AppUserModelId on your application's shortcut!
                    ToastNotificationManager.CreateToastNotifier(APP_ID).Show(toast);
                }

              
                if (contador > totalml * 2)
                {
                  
                    wow.Visibility = Visibility.Visible;
                    haha.Visibility = Visibility.Collapsed;
                    triste.Visibility = Visibility.Collapsed;
                    zen1.Visibility = Visibility.Collapsed;
                    zen2.Visibility = Visibility.Collapsed;

                    // Get a toast XML template
                    Windows.Data.Xml.Dom.XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText03);

                    // Fill in the text elements
                    Windows.Data.Xml.Dom.XmlNodeList stringElements = toastXml.GetElementsByTagName("text");
                    stringElements[0].AppendChild(toastXml.CreateTextNode("Beba Água APP"));
                    stringElements[1].AppendChild(toastXml.CreateTextNode(mensagem5));



                    // Specify the absolute path to an image
                    String imagePath = "file:///" + Path.GetFullPath(@"/bebaagua.jpg");
                    Windows.Data.Xml.Dom.XmlNodeList imageElements = toastXml.GetElementsByTagName("image");
                    imageElements[0].Attributes.GetNamedItem("src").NodeValue = imagePath;

                    // Create the toast and attach event listeners
                    ToastNotification toast = new ToastNotification(toastXml);

                    // Show the toast. Be sure to specify the AppUserModelId on your application's shortcut!
                    ToastNotificationManager.CreateToastNotifier(APP_ID).Show(toast);

                }

                if (contador >= 10000)
                {                 
                    contador = 10000;
                    wow.Visibility = Visibility.Visible;
                    haha.Visibility = Visibility.Collapsed;
                    triste.Visibility = Visibility.Collapsed;
                    zen1.Visibility = Visibility.Collapsed;
                    zen2.Visibility = Visibility.Collapsed;
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
                try
                {
                    DadosAgua dad = new DadosAgua();
                    dad.Id = Convert.ToInt32(2);
                    dad.Contador = "0";
                    dad.PegaAguaValor = "0";

                    DalHelper.Update2(dad);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro : " + ex.Message);
                }


            contador = 0;
            Pegaaguavalor = 0;         
            pegaAgua.Text = "";
            textContador.Text = "0";
            haha.Visibility = Visibility.Collapsed;
            wow.Visibility = Visibility.Collapsed;
            triste.Visibility = Visibility.Visible;
            zen1.Visibility = Visibility.Collapsed;
            zen2.Visibility = Visibility.Collapsed;
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