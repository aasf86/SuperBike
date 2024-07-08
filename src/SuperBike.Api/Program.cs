using Microsoft.AspNetCore.Localization;
using SuperBike.Auth.Config;
using SuperBike.Business.Contracts.UseCases.User;
using SuperBike.Business.UseCases.User;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Gest�o de autentica��o e autoriza��o do super bike
builder.AddAuthSuperBike();

//Inje��o de depend�ncias
builder.Services.AddScoped<IUserUseCase, UserUseCase>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
