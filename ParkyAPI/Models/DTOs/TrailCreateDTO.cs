using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static ParkyAPI.Models.Trail;

namespace ParkyAPI.Models.DTOs;

public class TrailCreateDTO
{
    [Required]
    public string Name { get; set; }

    [Required]
    public double Distance { get; set; }
    [Required]
    public double Elevation { get; set; }

    public DifficultyType Difficulty { get; set; }

    [Required]
    public int NationalParkId { get; set; }

    //public NationalParkDTO NationalPark { get; set; }
}
