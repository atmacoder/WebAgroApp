 using System.ComponentModel.DataAnnotations;

namespace WebAgroApp.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; } // Хэшированный пароль 

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
