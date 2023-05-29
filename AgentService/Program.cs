using AgentService.AsyncDataServices;
using AgentService.Data;
using AgentService.SyncDataServices.Grpc;
using AgentService.SyncDataServices.Http;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
if (builder.Environment.IsProduction()) {
    Console.WriteLine("--> In Production");
    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseNpgsql(builder.Configuration.GetConnectionString("AgentsConnection")));
} else {
    Console.WriteLine("--> In Development");
    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseInMemoryDatabase("InMem"));
}

builder.Services.AddHttpClient<IEquipmentDataClient, HttpEquipmentDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
builder.Services.AddGrpc();

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
app.MapGrpcService<GrpcAgentService>();
app.MapGet("/protos/agents.proto", async context => {
    await context.Response.WriteAsync(File.ReadAllText("Protos/agents.proto"));
});

PrepDb.prepPopulation(app, app.Environment.IsProduction());

app.Run();
