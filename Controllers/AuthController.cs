using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAgroApp.Services; // Для IUserService
using WebAgroApp.Models; // Для RefreshToken, UserModel
using Microsoft.EntityFrameworkCore; // Добавьте эту директиву
using Microsoft.Extensions.Logging;

namespace WebAgroApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration; // Для получения секретного ключа
        private readonly IUserService _userService; // Сервис для аутентификации и регистрации
        private readonly ILogger<AuthController> _logger; // Добавьте ILogger

        public AuthController(IConfiguration configuration, IUserService userService, ILogger<AuthController> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _userService = userService;
            _logger = logger;
        }

        // Метод для логина (логина пользователя)
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            // Проверка учетных данных 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Проверка учетных данных
            var user = await _userService.Authenticate(loginModel.Email, loginModel.Password);
            if (user == null)
            {
                return Unauthorized(); // Неверные учетные данные
            }

            // Генерация JWT-токена
            var token = GenerateJwtToken(user);

            // Создание RefreshToken (если требуется)
            var refreshToken = _userService.GenerateRefreshToken(); // Функция генерации RefreshToken
            await _userService.SaveRefreshToken(user.Id, refreshToken);

            // Возвращаем токен 
            return Ok(new { token, refreshToken });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            _logger.LogInformation("Начало метода Register");

            // Проверка валидации модели 
            if (!ModelState.IsValid)
            {
                _logger.LogError("Ошибка валидации модели: {0}", ModelState.Values);
                return BadRequest(ModelState);
            }

            // Создание нового пользователя 
            var userModel = new UserModel
            {
                Email = registerModel.Email,
                Password = registerModel.Password,
                Name = registerModel.Name
            };

            _logger.LogInformation("Создание объекта userModel: {0}", userModel);

            var user = await _userService.Register(userModel); // Передаем userModel

            _logger.LogInformation("Результат вызова _userService.Register: {0}", user);

            if (user == null)
            {
               //_logger.LogError("Ошибка регистрации");
                return BadRequest("Пользователь с таким email уже существует");
            }

            // Генерация JWT-токена
            var token = GenerateJwtToken(user);

            _logger.LogInformation("Генерация токена: {0}", token);

            // Возвращаем токен 
            return Ok(new { token });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenModel refreshTokenModel)
        {
            // Проверка RefreshToken
            var refreshToken = await _userService.GetRefreshToken(refreshTokenModel.UserId);
            if (refreshToken == null || refreshToken.Token != refreshTokenModel.Token || refreshToken.Expires < DateTime.Now)
            {
                return Unauthorized("Недействительный RefreshToken");
            }

            // Генерация нового JWT-токена
            var user = await _userService.GetUserById(refreshTokenModel.UserId);
            var token = GenerateJwtToken(user);

            // Генерация нового RefreshToken
            var newRefreshToken = _userService.GenerateRefreshToken(); // Вызываем метод из UserService

            // Сохраняем новый RefreshToken 
            await _userService.SaveRefreshToken(user.Id, newRefreshToken);

            // Возвращаем новый токен
            return Ok(new { token, newRefreshToken });
        }

        // Метод для генерации JWT-токена
        private string GenerateJwtToken(UserModel user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            // Проверка конфигурации
            var secretKey = _configuration["JwtSecret"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new Exception("Секретный ключ JwtSecret не задан в конфигурации.");
            }

            var issuer = _configuration["Jwt:Issuer"];
            if (string.IsNullOrEmpty(issuer))
            {
                throw new Exception("Issuer не задан в конфигурации.");
            }

            var audience = _configuration["Jwt:Audience"];
            if (string.IsNullOrEmpty(audience))
            {
                throw new Exception("Audience не задан в конфигурации.");
            }

            // Создаем Claims (утверждения)
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                // Добавьте другие Claims, например, роль: new Claim(ClaimTypes.Role, "Администратор") 
            };

            // Создаем ключ для подписи 
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecret"]));
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]));
            // Создаем JWT-токен
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"], // Издатель
                audience: _configuration["Jwt:Audience"], // Получатель
                claims: claims,
                expires: DateTime.Now.AddMinutes(30), // Срок действия 30 минут
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            // Возвращаем строковое представление токена 
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Метод для генерации RefreshToken
        // private string GenerateRefreshToken()
        // {
        //     // ... (реализация генерации случайного RefreshToken)
        // }
    }

}