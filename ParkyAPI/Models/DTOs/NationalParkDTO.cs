using System.ComponentModel.DataAnnotations;

namespace ParkyAPI.Models.DTOs;

public class NationalParkDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string State { get; set; }
    public DateTime Created { get; set; }
    public DateTime Established { get; set; }
}
