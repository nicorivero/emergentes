using System.Text;
using RabbitMQ.Client;

namespace api_tecn_emergentes.Models
{
    public class RabbitMQ
    {
        public RabbitMQ()
        {
        }

        public void PostMessage(string _msj, string _queue)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection= factory.CreateConnection())
            {
                using (var channel= connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: _queue,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );
                    
                    var body = Encoding.UTF8.GetBytes(_msj);

                    channel.BasicPublish(
                        exchange:"",
                        routingKey: _queue,
                        basicProperties: null,
                        body: body
                    );
                }
            }
        }  
    }
}