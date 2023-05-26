using System.Text;
using System.Text.Json;
using AgentService.Dtos;
using RabbitMQ.Client;

namespace AgentService.AsyncDataServices;

public class MessageBusClient : IMessageBusClient {
    private readonly IConfiguration configuration;
    private readonly IConnection connection;
    private readonly IModel channel;

    public MessageBusClient(IConfiguration configuration) {
        this.configuration = configuration;
        var factory = new ConnectionFactory() {
            HostName = configuration["RabbitMQHost"],
            Port = int.Parse(configuration["RabbitMQPort"])
        };
        
        try {
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            
            channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            
            connection.ConnectionShutdown += rabbitMQ_ConnectionShutdown;
            
            Console.WriteLine("--> Connected to Message Bus");
        } catch (Exception e) {
            Console.WriteLine($"--> Could not connect to Message Bus: {e.Message}");
        }
    }
    
    public void publishNewAgent(AgentPublishDto agentPublishDto) {
        var message = JsonSerializer.Serialize(agentPublishDto);
        if (connection.IsOpen) {
            Console.WriteLine("--> RabbitMQ Connection Open, sending message...");
            sendMessage(message);
        } else {
            Console.WriteLine("--> RabbitMQ Connection Closed, not sending");
        }
    }

    private void sendMessage(string message) {
        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(
            exchange: "trigger",
            routingKey: "",
            basicProperties: null,
            body: body
        );
        Console.WriteLine($"--> Message published: {message}");
    }
    
    public void dispose() {
        Console.WriteLine("MessageBus Disposed");
        if (!channel.IsOpen) return;
        channel.Close();
        connection.Close();
    }

    private static void rabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) {
        Console.WriteLine("--> RabbitMQ Connection Shutdown");
    }
}