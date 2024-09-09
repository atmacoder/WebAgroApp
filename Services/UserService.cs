using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using WebAgroApp.Models;
using BCrypt.Net;

namespace WebAgroApp.Services
{
    public class UserService : IUserService
    {
        private readonly MyDbContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(MyDbContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<UserModel> Authenticate(string email, string password)
        {
            // Найдите пользователя по имени
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return null;
            }

            // Проверьте пароль с помощью BCrypt.Net
            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return null;
            }

            // **Важно: Заполните Id и Email в модели UserModel**
            var userModel = new UserModel
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name
            };

            return userModel;
        }

        public async Task<UserModel> Register(UserModel userModel)
        {
            // Проверка, существует ли email:
            if (_context.Users.Any(u => u.Email == userModel.Email))
            {
                return null; // Email уже существует
            }

            // Сохранение нового пользователя:
            userModel.Password = BCrypt.Net.BCrypt.HashPassword(userModel.Password); // Шифрование пароля

            _context.Users.Add(userModel);
            try
            {
                await _context.SaveChangesAsync();
                return userModel;
            }
            catch (Exception ex)
            {
                // Логирование ошибки:
                _logger.LogError(ex, "Ошибка сохранения пользователя"); // Используйте ILogger
                return null; // Возвращение null при ошибке
            }
        }

        public async Task<bool> SaveRefreshToken(int userId, string refreshToken)
        {
            // Проверьте, существует ли уже RefreshToken для этого пользователя
            var existingRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == userId);
            if (existingRefreshToken != null)
            {
                // Обновите существующий RefreshToken
                existingRefreshToken.Token = refreshToken;
                existingRefreshToken.Expires = DateTime.Now.AddDays(30);
            }
            else
            {
                // Создайте новый RefreshToken
                var refreshTokenEntity = new RefreshToken
                {
                    UserId = userId,
                    Token = refreshToken,
                    Expires = DateTime.Now.AddDays(30) // Срок действия 30 дней
                };
                _context.RefreshTokens.Add(refreshTokenEntity);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<RefreshToken> GetRefreshToken(int userId)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == userId);
        }

        // (Дополнительно) - Метод для генерации RefreshToken
        public string GenerateRefreshToken()
        {
            // Сгенерируйте случайную строку длиной 32 символа
            byte[] randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }
        public async Task<UserModel> GetUserById(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return null;
            }

            // **Важно: Заполните Id и Email в модели UserModel**
            var userModel = new UserModel
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name
            };

            return userModel;
        }

        public async Task<bool> DeleteRefreshToken(int userId)
        {
            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == userId);
            if (refreshToken == null)
            {
                return false;
            }

            _context.RefreshTokens.Remove(refreshToken);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ValidateRefreshToken(string refreshToken)
        {
            var tokenEntity = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshToken);
            if (tokenEntity == null || tokenEntity.Expires < DateTime.Now)
            {
                return false;
            }

            return true;
        }
    }
}