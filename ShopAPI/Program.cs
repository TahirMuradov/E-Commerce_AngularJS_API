
using Microsoft.AspNetCore.Identity;
using Shop.Application;
using Shop.Domain.Entities;
using Shop.Infrastructure;
using Shop.Persistence;
using Shop.Persistence.Context;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Configuration.Initialize(builder.Configuration);

builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();


builder.Services.AddIdentity<User, Role>()
.AddEntityFrameworkStores<AppDBContext>()
.AddDefaultTokenProviders();
builder.Services.AddControllers();




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
