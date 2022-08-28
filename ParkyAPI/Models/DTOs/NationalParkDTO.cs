using System.ComponentModel.DataAnnotations;

namespace ParkyAPI.Models.DTOs;

public class NationalParkDTO
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = "";
    [Required]
    public string State { get; set; } = "";
    public DateTime Created { get; set; }
    //public byte[] Picture { get; set; }
    public DateTime Established { get; set; }
}

