using Microsoft.AspNetCore.Localization;
using Serilog;
using SuperBike.Api.Config;
using SuperBike.Auth.Config;
using SuperBike.Business.Contracts.UseCases.User;
using SuperBike.Business.UseCases.User;
using SuperBike.Domain.Contracts.Repositories.Motorcycle;
using SuperBike.Infrastructure.Repositories.Motorcycle;
using System.Data;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using SuperBike.Auth.Context;
using Microsoft.EntityFrameworkCore;
using SuperBike.Business.Contracts.UseCases.Motorcycle;
using SuperBike.Business.UseCases.Motorcycle;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseSerilog();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenSuperBike();

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

Log.Information("Inicial SuperBike.Api: {Name}", Environment.UserName);

var app = builder.Build();

app.UseWhen(context=> 
{
    var ct = context;
    return true;
}, (build) => 
{ 
    var context = build;

});

app.Use((context, next) => 
{
    Log.Information("[#] Iniciando request");
    var result = next(context);
    Log.Information("[#] Finalizando request");
    return result;
});

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
