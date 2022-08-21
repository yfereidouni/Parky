using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static ParkyAPI.Models.Trail;

namespace ParkyAPI.Models.DTOs;

public class TrailDTO
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public double Distance { get; set; }

    public DifficultyType Difficulty { get; set; }

    [Required]
    public int NationalParkId { get; set; }

    public NationalParkDTO NationalPark { get; set; }
}
