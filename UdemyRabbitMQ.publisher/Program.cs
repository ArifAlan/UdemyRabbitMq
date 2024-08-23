using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;
using System.Threading.Channels;

namespace UdemyRabbitMQ.publisher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://bqxqfvzg:gw3_bkaaGY-QlejPK_bAPBgA7nJJ9NDO@codfish.rmq.cloudamqp.com/bqxqfvzg");

            using var connection = factory.CreateConnection();
            var channel=connection.CreateModel();
            //channel.QueueDeclare("hello-queue",true,false,false);//bir kuyruğu (queue) tanımlamak ve oluşturmak için kullanılan bir yöntemdir.

            channel.ExchangeDeclare("logs-fanout", durable: true,type:ExchangeType.Fanout);//channel.ExchangeDeclare("logs") RabbitMQ'da bir exchange (değişim) oluşturmak için kullanılan bir komuttur. RabbitMQ, mesajları yönlendiren bir mesaj kuyruğu sistemidir ve exchange'lar bu mesajların hangi kuyruklara yönlendirileceğini belirler.
            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {

                string message = $"log {x}";
                var messageBody = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("logs-fanout", "", null, messageBody);

                Console.WriteLine($"mesaj gönderilmiştir: {message}");
            });


            Console.ReadLine();

        }
    }
}
// Kuyruğu oluşturur
//channel.QueueDeclare(
//    queue: queueName,    // Kuyruğun adı
//    durable: durable,    // Kuyruğun kalıcılığı
//    exclusive: exclusive, // Kuyruğun özel olup olmadığı
//    autoDelete: autoDelete, // Kuyruğun otomatik silinip silinmeyeceği
//    arguments: arguments // Kuyruğun ek argümanları
//);
//BasicPublish: RabbitMQ'da bir mesajı bir kuyrukta veya bir exchange'de yaymak için kullanılan yöntemdir.

// Mesajı gönder
//channel.BasicPublish(
//    exchange: "",            // Boş exchange, doğrudan kuyruk hedeflenir
//    routingKey: "myQueue",  // Mesajın gönderileceği kuyruk adı
//    basicProperties: null,  // Mesajın özellikleri (null)
//    body: body              // Gönderilecek mesajın içeriği
//);