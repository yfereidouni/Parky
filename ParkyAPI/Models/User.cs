using System.ComponentModel.DataAnnotations.Schema;

namespace ParkyAPI.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    [NotMapped]
    public string Token { get; set; }
}
