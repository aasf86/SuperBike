using Serilog;
using SuperBike.Consumer.DataAccess;
using SuperBike.Consumer.ServiceHandler;
using SuperBike.Domain.Contracts.Services;
using SuperBike.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseSerilog();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IConfigurationManager>(src => builder.Configuration);

builder.Services.AddSingleton(src => FactoryDataAccess.CreateConnection(builder.Configuration.GetConnectionString("Default") ?? ""));
builder.Services.AddSingleton<DataAccessEvent>();
builder.Services.AddHostedService<ConsumerMessageBrocker>();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Seq(builder.Configuration.GetSection("ServerLog").Value ?? "")
    .Enrich.WithEnvironmentName()
    .Enrich.WithEnvironmentUserName()
    .Enrich.WithProcessName()
    .CreateBootstrapLogger();
//.CreateLogger();

Log.Information("Inicio SuperBike.Consumer: {Name}", Environment.UserName);

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment()) //==> Deixei Swagger visivel para apresentação/demonstração
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
