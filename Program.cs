using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAgroApp.Models;
using WebAgroApp.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Настройка сервисов
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserService, UserService>();

// Регистрация MyDbContext
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Настройка CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://localhost:5173")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Настройка аутентификации JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Проверка выдавшего токен
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // Выдавший токен

            ValidateAudience = true, // Проверка аудитории токена
            ValidAudience = builder.Configuration["Jwt:Audience"], // Аудитория токена

            ValidateLifetime = true, // Проверка срока действия токена

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"])), // Ключ для подписи токена
        };
    });

// Добавьте AddAuthorization()
builder.Services.AddAuthorization();

var app = builder.Build();

// Настройка конвейера HTTP-запросов
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseRouting();

// Вставьте этот код
app.UseAuthentication();
app.UseAuthorization();

app.UseCors("CorsPolicy"); // Включение CORS

// **Добавьте MyDbContext в конструктор AuthController**
app.MapGet("/", () => "Hello World!");

app.MapControllers()
    .WithMetadata(
        new FromServicesAttribute()
    );

app.Run();