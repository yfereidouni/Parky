using System.ComponentModel.DataAnnotations;

namespace ParkyAPI.Models;

public class NationalPark
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string State { get; set; }
    public DateTime Created { get; set; }
    public DateTime Established { get; set; }
}
