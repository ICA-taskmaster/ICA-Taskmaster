using System.Text;
using System.Text.Json;
using AgentService.Dtos;
using RabbitMQ.Client;

namespace AgentService.AsyncDataServices;

public class MessageBusClient : IMessageBusClient {
    private readonly IConfiguration configuration;
    private readonly IConnection connection;
    private readonly IModel channel;
    private readonly ILogger<MessageBusClient> logger;

    public MessageBusClient(IConfiguration configuration, ILogger<MessageBusClient> logger) {
        this.configuration = configuration;
        this.logger = logger;
        
        var factory = new ConnectionFactory {
            HostName = configuration["RabbitMQHost"],
            Port = int.Parse(configuration["RabbitMQPort"])
        };
        
        try {
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            
            channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            
            connection.ConnectionShutdown += rabbitMQ_ConnectionShutdown;
            
            logger.LogInformation("Connected to Message Bus");
        } catch (Exception e) {
            logger.LogWarning("Could not connect to Message Bus: {EMessage}", e.Message);
        }
    }
    
    public void publishNewAgent(AgentPublishDto agentPublishDto) {
        var message = JsonSerializer.Serialize(agentPublishDto);
        if (connection.IsOpen) {
            logger.LogInformation("RabbitMQ Connection Open, sending message...");
            sendMessage(message);
        } else {
            logger.LogWarning("RabbitMQ Connection Closed, not sending");
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
        logger.LogInformation("Message published: {Message}", message);
    }
    
    public void dispose() {
        logger.LogInformation("MessageBus Disposing");
        if (!channel.IsOpen) return;
        channel.Close();
        connection.Close();
    }

    private void rabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) {
        logger.LogCritical("RabbitMQ Connection Shutdown");
    }
}