namespace ParkyWeb.Models.ViewModel;

public class IndexVM
{
    public IEnumerable<NationalPark> NationalParkList { get; set; }
    public IEnumerable<Trail> TrailList { get; set; }
}
