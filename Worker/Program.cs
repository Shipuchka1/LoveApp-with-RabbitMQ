using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Worker
{

    class Program
    {
        public static List<Session> sessions = new List<Session>();
        static void Main(string[] args)
        {
            Thread worker = new Thread(delegate ()
            {
                Console.WriteLine("lll");
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("Q1", false, false, false, null);
                    channel.QueueDeclare("Q2", false, false, false, null);
                    channel.QueueDeclare("Q3", false, false, false, null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [x] Received {0}", message);
                        string[] ss = message.Split(' ');

                        if (ss[0] == "match")
                        {
                            Session s = new Session();
                            s.userId = Int32.Parse(ss[1]);
                            s.appId = ss[2];
                            s.loveNum = Int32.Parse(ss[3]);
                            sessions.RemoveAll(r => r.userId == s.userId);
                            List<Session> sess = sessions.Where(w => w.loveNum == s.loveNum).ToList();
                            string mes;
                            foreach (Session item in sess)
                            {
                                mes = item.userId + " " + item.appId + " " + s.userId + " " + s.appId;
                                var b = Encoding.UTF8.GetBytes(mes);
                                channel.BasicPublish("", "Q2", null, b);
                                channel.BasicPublish("", "Q3", null, b);
                                Console.WriteLine(String.Format("User match {0}", mes));
                                mes = s.userId + " " + s.appId + " " + item.userId + " " + item.appId;
                                b = Encoding.UTF8.GetBytes(mes);
                                channel.BasicPublish("", "Q2", null, b);
                            }
                            sessions.Add(s);

                        }
                        else if (ss[0] == "remove")
                        {
                            sessions.RemoveAll(r => r.appId == ss[1]);
                        }


                    };
                    channel.BasicConsume("Q1", true, consumer);

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();

                }

    }
                   );

            worker.Start();
        }

        
    }
    public class Session
    {
        public int userId { get; set; }
        public string appId { get; set; }
        public int loveNum { get; set; }

    }
}
