using Microsoft.EntityFrameworkCore;
using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Repository;

public class TrailRepository : ITrailRepository
{
    private readonly ApplicationDbContext _db;

    public TrailRepository(ApplicationDbContext context)
    {
        _db = context;
    }
    public bool CreateTrail(Trail trail)
    {
        _db.Trails.Add(trail);
        return Save();
    }

    public bool DeleteTrail(Trail trail)
    {
        _db.Trails.Remove(trail);
        return Save();
    }

    public Trail GetTrail(int trailId)
    {
        return _db.Trails.Include(c => c.NationalPark).FirstOrDefault(a => a.Id == trailId);
    }

    public ICollection<Trail> GetTrails()
    {
        return _db.Trails.Include(c => c.NationalPark).OrderBy(a => a.Name).ToList();
    }
    public ICollection<Trail> GetTrailsInNationalPark(int npId)
    {
        return _db.Trails.Include(c => c.NationalPark)
            .Where(c => c.NationalParkId == npId)
            .ToList();
    }

    public bool TrailExists(string name)
    {
        return _db.Trails.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
    }

    public bool TrailExists(int id)
    {
        return _db.Trails.Any(c => c.Id == id);
    }

    public bool Save()
    {
        return _db.SaveChanges() >= 0 ? true : false;
    }

    public bool UpdateTrail(Trail trail)
    {
        _db.Trails.Update(trail);
        return Save();
    }
}
