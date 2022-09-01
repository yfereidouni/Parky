using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ParkyWeb.Models.ViewModel;

public class TrailsVM
{
    //public List<SelectListItem> NationalParkList { get; set; }
    //public Trail Trail { get; set; }

    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public double Distance { get; set; }
    [Required]
    public double Elevation { get; set; }

    public enum DifficultyType { Easy, Moderate, Difficult, Expert }
    public DifficultyType Difficulty { get; set; }

    [Display(Name = "Select a National Park")]
    [Required(ErrorMessage = "National Park is required")]
    public int NationalParkId { get; set; }
}