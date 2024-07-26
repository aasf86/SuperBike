using Serilog;
using SuperBike.Api.FileStorage.DataAccess;
using SuperBike.Api.FileStorage.ServiceHandler;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseSerilog();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IConfigurationManager>(src => builder.Configuration);

builder.Services.AddSingleton(src => FactoryDataAccess.CreateConnection(builder.Configuration.GetConnectionString("Default") ?? ""));
builder.Services.AddSingleton<DataAccessFile>();
builder.Services.AddSingleton<FileManager>();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Seq(builder.Configuration.GetSection("ServerLog").Value ?? "")
    .Enrich.WithEnvironmentName()
    .Enrich.WithEnvironmentUserName()
    .Enrich.WithProcessName()
    .CreateBootstrapLogger();
//.CreateLogger();

Log.Information("Inicio SuperBike.Api.FileStorage: {Name}", Environment.UserName);

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment()) //==> Deixei Swagger visivel para apresentação/demonstração
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
