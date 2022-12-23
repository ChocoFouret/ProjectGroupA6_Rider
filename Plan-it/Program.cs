using System.Text;
using Application.UseCases.Accounts;
using Application.UseCases.Addresss;
using Application.UseCases.Announcements;
using Application.UseCases.Companies;
using Application.UseCases.Companies.Dtos;
using Application.UseCases.Events.Dtos;
using Application.UseCases.Functions;
using Application.UseCases.Has;
using Application.UseCases.Has.Dtos;
using Domain;
using Infrastructure;
using Infrastructure.EF;
using Infrastructure.EF.Companies;
using Infrastructure.EF.Events;
using Infrastructure.EF.Has;
using Infrastructure.EF.Session;
using JWT.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Plan_it;
using Plan_it.Controllers;
using Service.UseCases.Address;
using Service.UseCases.Companies;
using Service.UseCases.Has;
using WebSocketDemo.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IConnectionStringProvider, ConnectionStringProvider>();
builder.Services.AddScoped<IAccountRepository, EfAccountRepository>();
builder.Services.AddScoped<IFunctionRepository, EfFunctionRepository>();
builder.Services.AddScoped<ICompaniesRepository,EfCompaniesRepository>();
builder.Services.AddScoped<IHasRepository, EfHasRepository>();
builder.Services.AddScoped<IEventsRepository, EfEventsRepository>();
builder.Services.AddScoped<IEventTypesRepository, EfEventTypesRepository>();
builder.Services.AddScoped<IAddressRepository, EfAddressRepository>();
builder.Services.AddScoped<IAnnouncementsRepository, EfAnnouncementsRepository>();

//Use Case Has
builder.Services.AddScoped<UseCaseFetchAllHas>();
builder.Services.AddScoped<UseCaseFetchHasByCompanies>();
builder.Services.AddScoped<UseCaseFetchHasByAccount>();
builder.Services.AddScoped<UseCaseFetchHasByFunctions>();
builder.Services.AddScoped<UseCaseCreateHas>();
builder.Services.AddScoped<UseCaseFetchHasById>();
builder.Services.AddScoped<UseCaseDeleteHas>();
builder.Services.AddScoped<AccountController>();

//User cases Companies
builder.Services.AddScoped<UseCaseFetchAllCompanies>();
builder.Services.AddScoped<UseCaseCreateCompanies>();
builder.Services.AddScoped<UseCaseDeleteCompanies>();
builder.Services.AddScoped<UseCaseUpdateCompanies>();
builder.Services.AddScoped<UseCaseFetchCompaniesByName>();
builder.Services.AddScoped<UseCaseFetchCompaniesById>();
builder.Services.AddScoped<UseCaseFetchCompaniesByEmail>();
builder.Services.AddScoped<UseCaseJoinCompany>();

// Use cases accounts
builder.Services.AddScoped<UseCaseLoginAccount>();
builder.Services.AddScoped<UseCaseCreateAccount>();
builder.Services.AddScoped<UseCaseUpdateAccount>();
builder.Services.AddScoped<UseCaseDeleteAccount>();
builder.Services.AddScoped<UseCaseFetchAllAccounts>();
builder.Services.AddScoped<UseCaseFetchAccountByEmail>();
builder.Services.AddScoped<UseCaseFetchProfilById>();

// Use cases events
builder.Services.AddScoped<UseCaseCreateEvents>();
builder.Services.AddScoped<UseCaseDeleteEvents>();
builder.Services.AddScoped<UseCaseFetchAllEvents>();
builder.Services.AddScoped<UseCaseFetchEventsById>();
builder.Services.AddScoped<UseCaseFetchFromToEvents>();
builder.Services.AddScoped<UseCaseFetchStartToEndAccountEvents>();
builder.Services.AddScoped<UseCaseUpdateEvents>();
builder.Services.AddScoped<UseCaseFetchEventsByEmployee>();

// Use cases eventTypes
builder.Services.AddScoped<UseCaseCreateEventTypes>();
builder.Services.AddScoped<UseCaseUpdateEventTypes>();
builder.Services.AddScoped<UseCaseFetchEventTypesByType>();
builder.Services.AddScoped<UseCaseFetchAllEventType>();

// Use cases functions
builder.Services.AddScoped<UseCaseFetchAllFunctions>();
builder.Services.AddScoped<UseCaseCreateFunction>();
builder.Services.AddScoped<UseCaseFetchFunctionById>();

// Uses cases address
builder.Services.AddScoped<UseCaseFetchAllAddress>();
builder.Services.AddScoped<UseCaseFetchAddressById>();
builder.Services.AddScoped<UseCaseFetchAddressByPostCode>();
builder.Services.AddScoped<UseCaseCreateAddress>();
builder.Services.AddScoped<UseCaseUpdateAddress>();

// Uses cases announcements
builder.Services.AddScoped<UseCaseFetchAllByCompanyAnnouncements>();
builder.Services.AddScoped<UseCaseUpdateAnnouncements>();
builder.Services.AddScoped<UseCaseFetchAnnouncementsById>();
builder.Services.AddScoped<UseCaseFetchAnnouncementsByIdFunction>();
builder.Services.AddScoped<UseCaseCreateAnnouncements>();
builder.Services.AddScoped<UseCaseDeleteAnnouncements>();

// Database
builder.Services.AddScoped<IConnectionStringProvider, ConnectionStringProvider>();

// Context
builder.Services.AddScoped<PlanitContextProvider>();

/* It allows the frontend to access the backend. */
builder.Services.AddCors(options =>
{
    options.AddPolicy("Dev", policyBuilder =>
    {
        policyBuilder.WithOrigins("https://localhost:4200")
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
    options.AddPolicy("directors", policy => policy.RequireRole("Directeur"));
    options.AddPolicy("all", policy => policy.RequireRole("Employe", "Directeur"));
});
builder.Services.AddScoped<ISessionService, SessionService>();

builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Android Studio
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

/* It allows the frontend to access the backend. */
app.UseCors("Dev");

app.UseCookiePolicy();

app.UseWebSockets(new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(10),
});

app.UseRouting();

// Authentification
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<EventsHub>("/EventHub");
});


app.Run();