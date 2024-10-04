using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Threading.Channels;
using UdemyRabbitMQWeb.Watermark.Services;

namespace UdemyRabbitMQWeb.Watermark.Services
{
    public class RabbitMQClientService : IDisposable
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;
        public static string ExchangeName = "ImageDirectExchange";
        public static string RoutingWatermark = "watermark-route-image";
        public static string QueueName = "queue-watermark-image";

        private readonly ILogger<RabbitMQClientService> _logger;

        public RabbitMQClientService(ConnectionFactory connectionFactory, ILogger<RabbitMQClientService> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;

        }

        public IModel Connect()
        {
            _connection = _connectionFactory.CreateConnection();


            if (_channel is { IsOpen: true })
            {
                return _channel;
            }

            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(ExchangeName, type: "direct", true, false);

            _channel.QueueDeclare(QueueName, true, false, false, null);


            _channel.QueueBind(exchange: ExchangeName, queue: QueueName, routingKey: RoutingWatermark);

            _logger.LogInformation("RabbitMQ ile bağlantı kuruldu...");


            return _channel;

        }

        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();

            _connection?.Close();
            _connection?.Dispose();

            _logger.LogInformation("RabbitMQ ile bağlantı koptu...");

        }
    }
}
//private readonly ConnectionFactory _connectionFactory;

//Açıklama: Bu alan, RabbitMQ'ya bağlantı kurmak için kullanılan ConnectionFactory nesnesini temsil eder. Bu nesne, RabbitMQ'ya nasıl bağlanılacağıyla ilgili konfigürasyon bilgilerini içerir.
//private IConnection _connection;

//Açıklama: RabbitMQ sunucusuna olan bağlantıyı temsil eder. IConnection arabirimi, RabbitMQ ile olan bağlantıyı yönetir.
//private IModel _channel;

//Açıklama: RabbitMQ'daki bir iletişim kanalını temsil eder. Bu kanal üzerinden mesajlar gönderilir veya alınır. IModel arabirimi, bu kanalda yapılacak işlemleri tanımlar.
//public static string ExchangeName = "ImageDirectExchange";

//Açıklama: RabbitMQ'da kullanılacak olan "exchange"ın adını belirtir. Exchange, mesajların hangi kuyruklara yönlendirileceğini belirler.
//public static string RoutingWatermark = "watermark-route-image";

//Açıklama: Mesajların yönlendirilmesi için kullanılan "routing key"i belirtir. Bu anahtar, mesajların hangi kuyruklara yönlendirileceğini belirlemede kullanılır.
//public static string QueueName = "queue-watermark-image";

//Açıklama: RabbitMQ'daki kuyruğun adını belirtir. Kuyruklar, mesajları geçici olarak saklar ve tüketiciler tarafından işlenmesini sağlar.
//private readonly ILogger<RabbitMQClientService> _logger;

//Açıklama: Uygulamanın loglarını tutmak için kullanılan logger nesnesini temsil eder. Hata ayıklama ve izleme için log mesajları oluşturur.

//------------------
//Connect Metodu
//_connection = _connectionFactory.CreateConnection();: Bağlantı oluşturur.
//if (_channel is { IsOpen: true }): Daha önce açık bir kanal varsa, mevcut kanalı döndürür.
//_channel = _connection.CreateModel();: Yeni bir kanal oluşturur.
//_channel.ExchangeDeclare(ExchangeName, type: "direct", true, false);: Belirtilen exchange'ı tanımlar (type: "direct" bu exchange'ın doğrudan yönlendirme yapacağını belirtir).
//_channel.QueueDeclare(QueueName, true, false, false, null);: Belirtilen kuyruğu tanımlar.
//_channel.QueueBind(exchange: ExchangeName, queue: QueueName, routingKey: RoutingWatermark);: Kuyruğu belirli bir exchange ile bağlar ve routing key kullanır.
//_logger.LogInformation("RabbitMQ ile bağlantı kuruldu...");: Bağlantı kurulduğuna dair log mesajı kaydeder.
//return _channel;: Oluşturulan veya mevcut kanalı döndürür.