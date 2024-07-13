using Microsoft.EntityFrameworkCore;
using Serilog;
using SuperBike.Api.Config;
using SuperBike.Auth.Config;
using SuperBike.Auth.Context;
using SuperBike.Business.Contracts.UseCases.Motorcycle;
using SuperBike.Business.Contracts.UseCases.User;
using SuperBike.Business.UseCases.Motorcycle;
using SuperBike.Business.UseCases.User;
using SuperBike.Domain.Contracts.Repositories.Motorcycle;
using SuperBike.Infrastructure.Repositories.Motorcycle;
using System.Data;

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

//Injeção de dependências
builder.Services.AddScoped<IDbConnection>(src => {    
    var context = src.GetRequiredService<AuthIdentityDbContext>();
    return context.Database.GetDbConnection();
});
builder.Services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
builder.Services.AddScoped<IUserUseCase, UserUseCase>();
builder.Services.AddScoped<IMotorcycleUseCase, MotorcycleUseCase>();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341")
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
