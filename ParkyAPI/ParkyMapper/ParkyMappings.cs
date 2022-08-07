using AutoMapper;
using ParkyAPI.Models;
using ParkyAPI.Models.DTOs;

namespace ParkyAPI.ParkyMapper;

public class ParkyMappings: Profile
{
    public ParkyMappings()
    {
        CreateMap<NationalPark, NationalParkDTO>().ReverseMap();
    }
}
