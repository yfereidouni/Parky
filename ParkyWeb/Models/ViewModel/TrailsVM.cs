using Microsoft.AspNetCore.Mvc.Rendering;

namespace ParkyWeb.Models.ViewModel;

public class TrailsVM
{
    public IEnumerable<SelectListItem> NationalParkList { get; set; }
    public Trail Trail { get; set; }
}
