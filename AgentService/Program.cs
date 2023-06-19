using AgentService.AsyncDataServices;
using AgentService.Data;
using AgentService.SyncDataServices.Http;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

var logFilePath = Path.Combine("Logs", "logs.txt");
var logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
    .CreateLogger();
try {
    logger.Information("Starting web host");
} catch (Exception ex) {
    logger.Fatal(ex, "Host terminated unexpectedly");
}finally {
    Log.CloseAndFlush();
}

// Add services to the container.
if (builder.Environment.IsProduction()) {
    logger.Information("--> In Production");
    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseNpgsql(builder.Configuration.GetConnectionString("AgentsConnection")));
} else {
    logger.Information("--> In Development");
    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseInMemoryDatabase("InMem"));
}

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddHttpClient<IEquipmentDataClient, HttpEquipmentDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAgentRepository, AgentRepository>();

logger.Information("Equipment Service Endpoint: {EquipmentServiceUrl}/api/c/agents/", builder.Configuration["EquipmentService:Url"]);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

PrepDb.prepPopulation(app, app.Environment.IsProduction(), app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("PrepDb"));

app.Run();
