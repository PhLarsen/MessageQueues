using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDemo1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Application B");
            //Receive("IDG");

            for (int i = 0; i < 100; i++)
            {
                Receive("nyKo");
            }
            
            Console.ReadLine();
        }


        public static void Send(string queue, string data)
        {
            using (IConnection connection = new ConnectionFactory().CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue, false, false, false, null);
                    channel.BasicPublish(string.Empty, queue, null, Encoding.UTF8.GetBytes(data));
                }
            }
        }

        public static void Receive(string queue)
        {
            // Need to setup the same connection to App A.
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "localhost";
            factory.Port = 5672;
            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {

                    channel.QueueDeclare(queue, false, false, false, null);
                    var consumer = new EventingBasicConsumer(channel);
                    BasicGetResult result = channel.BasicGet(queue, true);
                    if (result != null)
                    {
                        string data =
                        Encoding.UTF8.GetString(result.Body);
                        Console.WriteLine(data);
                        System.IO.File.WriteAllText(@"C:\AsyncAndAwait.txt", "This message is recieved by appliation B: " + data);

                    }
                }
            }

        }
    }
}
