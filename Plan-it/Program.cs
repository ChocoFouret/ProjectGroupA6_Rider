using Application.UseCases.Accounts;
using Application.UseCases.Functions;
using Domain;
using Infrastructure;
using Infrastructure.EF;
using Plan_it;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IConnectionStringProvider, ConnectionStringProvider>();
builder.Services.AddScoped<IAccountRepository, EfAccountRepository>();
builder.Services.AddScoped<IFunctionRepository, EfFunctionRepository>();

// Use cases accounts
builder.Services.AddScoped<UseCaseFetchAllAccounts>();
builder.Services.AddScoped<UseCaseCreateAccount>();
builder.Services.AddScoped<UseCaseFetchAccountById>();

// Use cases functions
builder.Services.AddScoped<UseCaseFetchAllFunctions>();
builder.Services.AddScoped<UseCaseCreateFunction>();
builder.Services.AddScoped<UseCaseFetchFunctionById>();

// Database
builder.Services.AddScoped<IConnectionStringProvider, ConnectionStringProvider>();

// Context
builder.Services.AddScoped<PlanitContextProvider>();


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