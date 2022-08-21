using ParkyAPI.Models;

namespace ParkyAPI.Repository.IRepository;

public interface ITrailRepository
{
    ICollection<Trail> GetTrails();
    ICollection<Trail> GetTrailsInNationalPark(int npId);
    Trail GetTrail(int trailId);
    bool TrailExists(string name);
    bool TrailExists(int id);
    bool CreateTrail(Trail trail);
    bool UpdateTrail(Trail trail);
    bool DeleteTrail(Trail trail);
    bool Save();
}
