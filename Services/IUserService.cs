using System.Threading.Tasks;
using WebAgroApp.Models;

namespace WebAgroApp.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Проверяет учетные данные пользователя.
        /// </summary>
        /// <param name="email">Адрес электронной почты пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <returns>Объект `UserModel` с данными пользователя, если аутентификация прошла успешно; 
        /// иначе `null`.</returns>
        Task<UserModel> Authenticate(string email, string password);

        /// <summary>
        /// Регистрирует нового пользователя.
        /// </summary>
        /// <param name="userModel">Модель данных пользователя для регистрации.</param>
        /// <returns>Объект `UserModel` с данными нового пользователя; 
        /// иначе `null` (если пользователь уже существует).</returns>
        Task<UserModel> Register(UserModel userModel);

        /// <summary>
        /// Сохраняет RefreshToken для пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="refreshToken">RefreshToken.</param>
        /// <returns>Значение `true`, если RefreshToken сохранен; иначе `false`.</returns>
        Task<bool> SaveRefreshToken(int userId, string refreshToken);

        /// <summary>
        /// Получает RefreshToken, связанный с пользователем.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>Объект `RefreshToken`, если он найден; иначе `null`.</returns>
        Task<RefreshToken> GetRefreshToken(int userId);

        /// <summary>
        /// Генерирует новый RefreshToken.
        /// </summary>
        /// <returns>Новый RefreshToken.</returns>
        string GenerateRefreshToken();

        /// <summary>
        /// Получает пользователя по его идентификатору.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>Объект `UserModel` с данными пользователя; 
        /// иначе `null`, если пользователь не найден.</returns>
        Task<UserModel> GetUserById(int userId);

        /// <summary>
        /// Удаляет RefreshToken, связанный с пользователем.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>Значение `true`, если RefreshToken успешно удален; иначе `false`.</returns>
        Task<bool> DeleteRefreshToken(int userId);

        /// <summary>
        /// Проверяет, действителен ли RefreshToken.
        /// </summary>
        /// <param name="refreshToken">RefreshToken для проверки.</param>
        /// <returns>Значение `true`, если RefreshToken действителен; иначе `false`.</returns>
        Task<bool> ValidateRefreshToken(string refreshToken);
    }
}