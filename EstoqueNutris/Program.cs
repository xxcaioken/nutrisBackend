﻿using EstoqueNutris.Data;
using EstoqueNutris.Models;
using EstoqueNutris.Repositories;
using EstoqueNutris.Repositories.Interfaces;
using EstoqueNutris.Services;
using EstoqueNutris.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Ativar logs detalhados de segurança (PII) - apenas para dev
IdentityModelEventSource.ShowPII = true;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<GoogleAuthService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<EscolaService>();
builder.Services.AddHttpClient();

// PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// JWT
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];

if (string.IsNullOrWhiteSpace(jwtKey) || jwtKey.Length < 32)
    throw new InvalidOperationException("Jwt:Key deve conter pelo menos 32 caracteres no appsettings.json.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    // LOG detalhado de erros JWT
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("⚠️ Falha na autenticação JWT: " + context.Exception);
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            Console.WriteLine("❌ JWT Challenge falhou: " + context.ErrorDescription);
            return Task.CompletedTask;
        }
    };

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
    };
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Registrar repositórios
builder.Services.AddScoped<IEscolaRepository, EscolaRepository>();
builder.Services.AddScoped<IUsuarioEscolaRepository, UsuarioEscolaRepository>();
builder.Services.AddScoped<ILinkRepository, LinkRepository>();
builder.Services.AddScoped<IImportacaoManagerRepository, ImportacaoManagerRepository>();

// Registrar serviços
builder.Services.AddScoped<IEscolaService, EscolaService>();
builder.Services.AddScoped<IUsuarioEscolaService, UsuarioEscolaService>();
builder.Services.AddScoped<ILinkService, LinkService>();
builder.Services.AddScoped<IImportacaoManagerService, ImportacaoManagerService>();

var app = builder.Build();

// Inicializa o banco de dados
using (var scope = app.Services.CreateScope())
{
    await DbInitializer.Initialize(scope.ServiceProvider);
}

// Inicializa as roles
using (var scope = app.Services.CreateScope())
{
    var roleService = scope.ServiceProvider.GetRequiredService<RoleService>();
    await roleService.InitializeRoles();
}

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); 
}

app.UseHttpsRedirection();

app.UseAuthentication(); // ⚠️ ordem importa
app.UseAuthorization();

app.MapControllers();

app.Run();
