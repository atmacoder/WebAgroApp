using System.ComponentModel.DataAnnotations;

public class RefreshToken
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }
    public string Token { get; set; }
    public DateTime Expires { get; set; }
}