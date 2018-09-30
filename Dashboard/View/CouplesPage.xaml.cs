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
using System.Windows.Navigation;
using System.Windows.Shapes;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Dashboard.Model;
using System.Threading;

namespace Dashboard.View
{
    /// <summary>
    /// Interaction logic for CouplesPage.xaml
    /// </summary>
    public partial class CouplesPage : Page
    {
        public static Model1 db = new Model1();
        public static StackPanel CoupleStack;
        public static SynchronizationContext UIContext;
        public CouplesPage()
        {
            InitializeComponent();
            CoupleStack = CouplesSP;
            UIContext = SynchronizationContext.Current;
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
           var channel = connection.CreateModel();
            int t = 0;
                channel.QueueDeclare("Q3", false, false, false, null);

                var consumer = new EventingBasicConsumer(channel);
                
                consumer.Received += (model, ea) =>
                {
                    t = 9;
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    string[] str = message.Split(' ');
                    int userId1 = Int32.Parse(str[0]);
                    int userId2 = Int32.Parse(str[2]);
                    Task<string> name1 = GetNameAsync(userId1);
                    Task<string> name2 = GetNameAsync(userId2);
                    name1.Wait();
                    name2.Wait();
                    string str1 = "Новая пара: " + name1.Result + " и " + name2.Result;
                    UIContext.Post(UIUpdate, str1);
                   
                   
                  

                };
                channel.BasicConsume("Q3", true, consumer);

            
            
        }

        public static void UIUpdate(object str)
        {
            TextBlock tb = new TextBlock();
            tb.Margin = new Thickness(20);
            tb.Height = 50;
            tb.FontSize = 30;
            tb.Text = str as string;
            CouplesPage.CoupleStack.Children.Add(tb);
           
        }

        static async Task<string> GetNameAsync(int user_id)
        {
            string name = db.Users.FirstOrDefault(f => f.user_id == user_id).name;
            return name;
        }
    }
}
