using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Application A");
            //Send("IDG", "This message is sent by application A");
            //Receive("IDG");
            //Send("nyKo", "Dette er en besked xd");
            for (int i = 0; i < 100; i++)
            {
                Send("nyKo", "HEJ DET ER MIG THOMAS");
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

            using (IConnection connection = new ConnectionFactory().CreateConnection())
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
                        System.IO.File.WriteAllText(@"C:\Example.txt", data);

                    }
                }
            }

        }
    }
}
