using AgentService.AsyncDataServices;
using AgentService.Data;
using AgentService.SyncDataServices.Http;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);
var logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
try {
    Log.Information("Starting web host");
} catch (Exception ex) {
    Log.Fatal(ex, "Host terminated unexpectedly");
}finally {
    Log.CloseAndFlush();
}

// Add services to the container.
if (builder.Environment.IsProduction()) {
    Log.Information("--> In Production");
    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseNpgsql(builder.Configuration.GetConnectionString("AgentsConnection")));
} else {
    Log.Information("--> In Development");
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

Console.WriteLine($"--> Equipment Service Endpoint: {builder.Configuration["EquipmentService:Url"]}/api/c/agents/");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

PrepDb.prepPopulation(app, app.Environment.IsProduction());

app.Run();
