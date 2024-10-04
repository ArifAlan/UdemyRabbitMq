using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace UdemyRabbitMQWeb.Watermark.Services
{
    public class RabbitMQPublisher
    {
        private readonly RabbitMQClientService _rabbitMQClientService;

        public RabbitMQPublisher(RabbitMQClientService rabbitMQClientService)
        {
            _rabbitMQClientService = rabbitMQClientService;
        }

        public void Publish(productImageCreatedEvent productImageCreatedEvent)
        {
            var channel = _rabbitMQClientService.Connect();

            var bodyString = JsonSerializer.Serialize(productImageCreatedEvent);

            var bodyByte = Encoding.UTF8.GetBytes(bodyString);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: RabbitMQClientService.ExchangeName, routingKey: RabbitMQClientService.RoutingWatermark, basicProperties: properties, body: bodyByte);

        }
    }
}
//RabbitMQPublisher Sınıfı

//private readonly RabbitMQClientService _rabbitMQClientService;
//Açıklama:
//private readonly RabbitMQClientService _rabbitMQClientService;: Bu alan, RabbitMQ'ya bağlantı ve mesaj gönderme işlemlerini yöneten RabbitMQClientService nesnesini tutar. readonly anahtar kelimesi, bu alanın yalnızca yapıcıda atanabileceğini ve daha sonra değiştirilemeyeceğini belirtir. Bu, nesnenin bağımlılıklarının sabit kalmasını sağlar.
//Yapıcı (Constructor)

//public RabbitMQPublisher(RabbitMQClientService rabbitMQClientService)
//{
//    _rabbitMQClientService = rabbitMQClientService;
//}
//Açıklama:
//public RabbitMQPublisher(RabbitMQClientService rabbitMQClientService): Bu, RabbitMQPublisher sınıfının yapıcı metodudur. rabbitMQClientService parametresi, RabbitMQClientService türündeki bir nesneyi temsil eder ve bu nesne, _rabbitMQClientService alanına atanır. Böylece RabbitMQPublisher sınıfı, gerekli bağımlılığı almış olur.
//Publish Metodu
//csharp
//Kodu kopyala
//public void Publish(productImageCreatedEvent productImageCreatedEvent)
//{
//    var channel = _rabbitMQClientService.Connect();
//Açıklama:
//    public void Publish(productImageCreatedEvent productImageCreatedEvent): Bu metod, productImageCreatedEvent türünde bir parametre alır ve herhangi bir değer döndürmez (void). Bu metod, belirli bir olayı RabbitMQ üzerinden yayımlamak için kullanılır.
//var channel = _rabbitMQClientService.Connect();: Connect metodu, _rabbitMQClientService üzerinden RabbitMQ sunucusuna bir bağlantı kurar ve bu bağlantının bir channel (kanal) nesnesini döner. Kanal, mesaj gönderme ve alma işlemleri için kullanılır.
//Mesajı Serileştirme
//csharp
//Kodu kopyala
//    var bodyString = JsonSerializer.Serialize(productImageCreatedEvent);
//var bodyByte = Encoding.UTF8.GetBytes(bodyString);
//Açıklama:
//var bodyString = JsonSerializer.Serialize(productImageCreatedEvent);: Bu satır, productImageCreatedEvent nesnesini JSON formatına dönüştürür. Bu, nesnenin metin temelli bir temsilini oluşturur.
//var bodyByte = Encoding.UTF8.GetBytes(bodyString);: bodyString değişkenindeki JSON metnini baytlara (byte array) dönüştürür. RabbitMQ, mesajları bayt olarak gönderdiği için bu dönüşüm gereklidir.
//Mesaj Özelliklerini Ayarlama
//csharp
//Kodu kopyala
//    var properties = channel.CreateBasicProperties();
//properties.Persistent = true;
//Açıklama:
//var properties = channel.CreateBasicProperties();: Bu satır, RabbitMQ mesajları için temel özellikler oluşturur. Mesajın gönderimi hakkında bilgi sağlar.
//properties.Persistent = true;: Bu özellik, mesajın kalıcı olmasını belirtir. Yani, RabbitMQ sunucusu kapansa bile mesaj kaybolmaz. Bu, mesajın güvenilir bir şekilde işlenmesini sağlamak için önemlidir.
//Mesajı Yayınlama
//csharp
//Kodu kopyala
//    channel.BasicPublish(exchange: RabbitMQClientService.ExchangeName, routingKey: RabbitMQClientService.RoutingWatermark, basicProperties: properties, body: bodyByte);
//}
//Açıklama:
//channel.BasicPublish(...): Bu metot, oluşturulan mesajı belirli bir exchange ve routing key ile yayımlar.
//exchange: RabbitMQClientService.ExchangeName: Hangi exchange üzerinden mesajın yayınlanacağını belirtir. Bu değer, RabbitMQClientService içinde tanımlanmış bir statik değişkendir.
//routingKey: RabbitMQClientService.RoutingWatermark: Mesajın hangi kuyrukta işleneceğini belirtir. Bu da RabbitMQClientService içinde tanımlanmış bir değerdir.
//basicProperties: properties: Daha önce oluşturduğumuz ve mesajın özelliklerini içeren nesnedir.
//body: bodyByte: Yayınlanacak olan mesajın bayt formatındaki halidir.
//Özet
//RabbitMQPublisher sınıfı, RabbitMQ üzerinden olayları yayımlamak için kullanılan bir yapıdadır. Yapıcıda bir RabbitMQClientService nesnesi alınır ve Publish metodu ile belirtilen olay nesnesi JSON formatında serileştirilip mesaj olarak RabbitMQ'ya gönderilir. Mesajın kalıcı olması için gerekli özellikler ayarlanır. Bu yapı, asenkron iletişim ve olay tabanlı mimari kurmak için oldukça yararlıdır.



