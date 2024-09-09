using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAgroApp.Models;
using Microsoft.EntityFrameworkCore;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly MyDbContext _context;

    public UserController(MyDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetUser()
    {
        // Получите текущего пользователя из контекста
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user != null)
        {
            // Создайте новый объект UserModelForResponse, чтобы не включать пароль в ответ
            var userForResponse = new UserModelForResponse
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                // другие поля, которые нужно вернуть
            };

            // Вернуть данные пользователя
            return Ok(userForResponse);
        }
        else
        {
            return NotFound(); // код 404, если пользователь не найден
        }
    }
}