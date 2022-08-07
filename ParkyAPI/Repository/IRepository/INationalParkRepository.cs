using ParkyAPI.Models;
using ParkyAPI.Models.DTOs;

namespace ParkyAPI.Repository.IRepository;

public interface INationalParkRepository
{
    ICollection<NationalPark> GetNationalParks();
    NationalPark GetNationalPark(int nationalParkId);
    bool NationalParkExists(string name);
    bool NationalParkExists(int id);
    bool CreateNationalPark(NationalPark nationalPark);
    bool UpdateNationalPark(NationalPark nationalPark);
    bool DeleteNationalPark(NationalPark nationalPark);
    bool Save();
}
