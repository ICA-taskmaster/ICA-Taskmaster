using System.Text;
using EquipmentService.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EquipmentService.AsyncDataServices;

public class MessageBusSubscriber : BackgroundService {
    private readonly IConfiguration configuration;
    private readonly IEvent eventProcessor;
    private IConnection connection;
    private IModel channel;
    private string queueName;

    public MessageBusSubscriber(IConfiguration configuration, IEvent eventProcessor) {
        this.configuration = configuration;
        this.eventProcessor = eventProcessor;
        initializeRabbitMq();
    }
    
    private void initializeRabbitMq() {
        var factory = new ConnectionFactory {
            HostName = configuration["RabbitMQHost"],
            Port = int.Parse(configuration["RabbitMQPort"])
        };
        connection = factory.CreateConnection();
        channel = connection.CreateModel();
        
        channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
        
        queueName = channel.QueueDeclare().QueueName;
        channel.QueueBind(
            queue: queueName, 
            exchange: "trigger",
            routingKey: ""
        );
        Console.WriteLine("--> Listening on the Message Bus...");
        
        connection.ConnectionShutdown += rabbitMQ_ConnectionShutdown;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken) {
        stoppingToken.ThrowIfCancellationRequested();
        
        var consumer = new EventingBasicConsumer(channel);
        
        consumer.Received += (ModuleHandle, ea) => {
            Console.WriteLine("--> Event received!");
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body.ToArray());
            eventProcessor.handleEvent(message);
        };
        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        
        return Task.CompletedTask;
    }
    
    public void dispose() {
        Console.WriteLine("MessageBus Disposed");
        if (!channel.IsOpen) return;
        channel.Close();
        connection.Close();
    }
    
    private static void rabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) {
        Console.WriteLine("--> Connection Shutdown");
    }
}