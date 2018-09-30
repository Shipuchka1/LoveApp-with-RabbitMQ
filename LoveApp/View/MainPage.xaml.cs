using LoveApp.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LoveApp.View
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public static StackPanel PartnerSP;
        public static Model1 db = new Model1();
        public static SynchronizationContext UIContext;
          public static ConnectionFactory factory;
        public static IConnection connection;
       public static IModel channel;
        
        public MainPage()
        {
            Application.Current.Exit += Current_Exit;
            InitializeComponent();
            PartnerSP = PartnerStackPanel;
            UIContext = SynchronizationContext.Current;
          factory = new ConnectionFactory() { HostName = "localhost" };
          connection  = factory.CreateConnection();
          channel = connection.CreateModel();
            
                channel.QueueDeclare("Q2", false, false, false, null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var b = ea.Body;
                    string m = Encoding.UTF8.GetString(b);
                    string[] str = m.Split(' ');
                    if (str.Length!=4) MessageBox.Show(string.Format("some error. count is {0}", str.Length));
                    if (Autorization.appId == str[1]||Autorization.appId==str[3])
                    {
                        int partnerId;
                        if(Autorization.userId == Int32.Parse(str[0]))
                            {
                            partnerId = Int32.Parse(str[2]);
                        }
                        else
                        {
                            partnerId = Int32.Parse(str[0]);
                        }
                        Partner p = new Partner();

                        Users u = db.Users.FirstOrDefault(f => f.user_id == partnerId);
                        if (u == null) MessageBox.Show(string.Format("some error. id is {0}", partnerId));
                        p.name = db.Users.FirstOrDefault(f => f.user_id == partnerId).name;
                        p.email = db.Users.FirstOrDefault(f => f.user_id == partnerId).email;
                        p.phone = db.Users.FirstOrDefault(f => f.user_id == partnerId).phone;

                        UIContext.Post(UpdateUI, p);
                        
                    }
                };
                channel.BasicConsume("Q2", true, consumer);
            
            }

        private void Current_Exit(object sender, ExitEventArgs e)
        {
            channel.QueueDeclare("Q1", false, false, false, null);

            string message = "remove "+Autorization.appId;
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("", "Q1", null, body);
            channel.Close();
            connection.Close();
           
        }

        private void SearchCoupleButton_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            int loveNum = rnd.Next(10);


            Task publish = PublishQ1Async(loveNum);
            

           
        }

        public static void UpdateUI(object Partner)
        {
            Partner p = Partner as Partner;

            Border bord = new Border();
            bord.BorderThickness = new Thickness(10);
            bord.BorderBrush = new SolidColorBrush(Colors.Violet);
            bord.Margin = new Thickness(10);

            StackPanel PartSP = new StackPanel();
            PartSP.VerticalAlignment = VerticalAlignment.Center;
            PartSP.HorizontalAlignment = HorizontalAlignment.Center;
            Color col = new Color();
            col.A = 255;
            col.R = 255;
            col.G = 186;
            col.B = 235;
            PartSP.Background = new SolidColorBrush(col);
            PartSP.Width = 300;
           

            bord.Child = PartSP;

            TextBlock nameTB = new TextBlock();
            nameTB.Margin = new Thickness(10);
            nameTB.Text = p.name;
            nameTB.FontSize = 35;
            PartSP.Children.Add(nameTB);

            TextBlock emailTB = new TextBlock();
            emailTB.Margin = new Thickness(10);
            emailTB.Text = p.email;
            PartSP.Children.Add(emailTB);

            TextBlock phoneTB = new TextBlock();
            phoneTB.Margin = new Thickness(10);
            phoneTB.Text = p.phone;
            PartSP.Children.Add(phoneTB);

            MainPage.PartnerSP.Children.Add(bord);
        }

        public async static Task PublishQ1Async(int loveNum)
        {
            channel.QueueDeclare("Q1", false, false, false, null);

            string message = "match "+ Autorization.userId + " " + Autorization.appId + " " + loveNum;
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("", "Q1", null, body);
            MessageBox.Show(String.Format("Отправлено число {0}. Ждите. Мы ищем пары для Вас", loveNum));
        }


    }

    public class Partner
    {
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }
}
