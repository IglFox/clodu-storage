using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Clodu.API.Data;
using Clodu.API.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ==================== СЕРВИСЫ ====================

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// PostgreSQL DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "Clodu",
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "CloduClient",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "clodu-super-secret-key-2024-very-long-and-secure"))
        };
    });

// Регистрация репозиториев
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();

builder.Services.AddAuthorization();

var app = builder.Build();

// ==================== MIDDLEWARE ====================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// ==================== ПРОВЕРКА БД ====================

using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.EnsureCreatedAsync();
        Console.WriteLine("✅ PostgreSQL подключена и готова к работе!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Ошибка подключения к PostgreSQL: {ex.Message}");
        Console.WriteLine("Убедитесь, что PostgreSQL запущен и настройки в appsettings.json верны.");
    }
}

// ==================== ЗАПУСК ====================

app.Run();