using Serilog;
using SuperBike.Api.Config;
using SuperBike.Auth.Config;
using SuperBike.Business.Config;
using SuperBike.Infrastructure.Config;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseSerilog();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenSuperBike();

#region SuperBike Setup/Config

//Gestão de autenticação e autorização do super bike
builder.AddAuthSuperBike();

//Injeção de dependências do super bike
builder.Services.AddBusinessIoC();
builder.Services.AddInfrastructureIoC(builder.Configuration);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Seq(builder.Configuration.GetSection("ServerLog").Value ?? "")
    .Enrich.WithEnvironmentName()
    .Enrich.WithEnvironmentUserName()
    .Enrich.WithProcessName()
    .CreateBootstrapLogger();
    //.CreateLogger();

Log.Information("Inicio SuperBike.Api: {Name}", Environment.UserName);

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{    
    app.UseSwagger();
    app.UseSwaggerUI();    
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();