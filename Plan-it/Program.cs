using System.Text;
using Application.UseCases.Accounts;
using Application.UseCases.Functions;
using Domain;
using Infrastructure;
using Infrastructure.EF;
using JWT.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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
builder.Services.AddScoped<UseCaseLoginAccount>();
builder.Services.AddScoped<UseCaseCreateAccount>();
builder.Services.AddScoped<UseCaseUpdateAccount>();
builder.Services.AddScoped<UseCaseDeleteAccount>();
builder.Services.AddScoped<UseCaseFetchAllAccounts>();
builder.Services.AddScoped<UseCaseFetchAccountById>();
builder.Services.AddScoped<UseCaseFetchAccountByEmail>();
builder.Services.AddScoped<UseCaseGetAccount>();

// Use cases functions
builder.Services.AddScoped<UseCaseFetchAllFunctions>();
builder.Services.AddScoped<UseCaseCreateFunction>();
builder.Services.AddScoped<UseCaseFetchFunctionByTitle>();

// Database
builder.Services.AddScoped<IConnectionStringProvider, ConnectionStringProvider>();

// Context
builder.Services.AddScoped<PlanitContextProvider>();

/* It allows the frontend to access the backend. */
builder.Services.AddCors(options =>
{
    options.AddPolicy("Dev", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Authentification
// Adding value into appsettings.json
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new
            SymmetricSecurityKey
            (Encoding.UTF8.GetBytes
                (builder.Configuration["Jwt:Key"]))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["session"];
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("director", policy => policy.RequireRole("Director"));
    options.AddPolicy("administrator", policy => policy.RequireRole("Administrator"));
    options.AddPolicy("all", policy => policy.RequireRole("Director", "Administrator"));
});
builder.Services.AddScoped<ISessionService, SessionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

/* It allows the frontend to access the backend. */
app.UseCors("Dev");

// Authentification
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();