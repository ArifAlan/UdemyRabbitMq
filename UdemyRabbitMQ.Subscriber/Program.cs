using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace UdemyRabbitMQ.Subscriber
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://bqxqfvzg:gw3_bkaaGY-QlejPK_bAPBgA7nJJ9NDO@codfish.rmq.cloudamqp.com/bqxqfvzg");

            using var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            // channel.QueueDeclare("hello-queue", true, false, false);


            var randomQueue = "log-database-save";//channel.QueueDeclare().QueueName;
            channel.QueueDeclare(randomQueue, true, false, false);

            channel.QueueBind(randomQueue, "logs-fanout","",null);

            channel.BasicQos(0,1,false );
            var consumer=new EventingBasicConsumer(channel);
            //Kuyruğu dinlemeye başla
            Console.WriteLine("Loglar dinleniyor.");
            channel.BasicConsume(randomQueue, false, consumer);
            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Thread.Sleep(1500);
                Console.WriteLine("Mesaj alındı: {0}", message);

                channel.BasicAck(e.DeliveryTag,false);//RabbitMQ'da bir mesajın başarıyla işlendiğini onaylamak için kullanılır. Bu metodun amacı, mesajın işleme alındığını ve kuyruktan kaldırılmasını belirtmektir. Bu, mesajın başarılı bir şekilde işlenip işlenmediğini belirlemek için kullanılır ve mesajın tekrar işlenmeyeceğini garanti eder.
            };


            Console.ReadLine();
            Console.WriteLine("Hello World!");
        }
    }
}

//string queue: "myQueue",        // Dinlenecek kuyruğun adı
//            autoAck: true,           // Mesajların otomatik olarak alındı olarak işaretlenmesi (true) veya manuel olarak işaretlenmesi (false)
//            consumerTag: "",         // Bu tüketici için benzersiz etiket. Boş bırakılırsa RabbitMQ otomatik bir etiket oluşturur.
//            noLocal: false,          // Bu tüketici aynı bağlantı üzerinden gönderilen mesajları almazsa true. Genellikle false olarak ayarlanır.
//            exclusive: false,        // Kuyruğun sadece bu tüketici tarafından kullanılmasını sağlar (true) veya diğer tüketicilerin de erişebilmesini sağlar (false)
//            noAck: false,           // Mesajların otomatik olarak alındı olarak işaretlenip işaretlenmeyeceğini belirtir. Genellikle autoAck ile birlikte kullanılır.
//            consumer: consumer       // Mesajları alacak ve işlemek üzere tanımlanmış EventingBasicConsumer nesnesi

// channel.QueueDeclare("hello-queue", true, false, false);//bir kuyruğu (queue) tanımlamak ve oluşturmak için kullanılan bir yöntemdir.
//  var consumer=new EventingBasicConsumer(channel);//RabbitMQ'dan mesajları alırken kullanıcının mesajları asenkron olarak işleyebilmesi için bir olay (event) tabanlı yapı sağlar.
//channel.BasicAck(e.DeliveryTag,false);//RabbitMQ'da bir mesajın başarıyla işlendiğini onaylamak için kullanılır. Bu metodun amacı, mesajın işleme alındığını ve kuyruktan kaldırılmasını belirtmektir. Bu, mesajın başarılı bir şekilde işlenip işlenmediğini belirlemek için kullanılır ve mesajın tekrar işlenmeyeceğini garanti eder.