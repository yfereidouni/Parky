using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.DTOs;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers;

//[Route("api/[controller]")]
[Route("api/v{version:apiVersion}/nationalparks")]
[ApiVersion("2.0")]
[ApiController]
//[ApiExplorerSettings(GroupName ="ParkyOpenAPISpecNP")]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
public class NationalParksV2Controller : ControllerBase
{
    private readonly INationalParkRepository _npRepo;
    private readonly IMapper _mapper;

    public NationalParksV2Controller(INationalParkRepository npRepo, IMapper mapper)
    {
        _npRepo = npRepo;
        _mapper = mapper;
    }

    /// <summary>
    /// Get list of National Parks.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(List<NationalParkDTO>))]
    public IActionResult GetNationalParks()
    {
        // Hint : We should NOT expose our domain model to MVC View.
        // So we send DTOs to the MVC View 
        var obj = _npRepo.GetNationalParks().FirstOrDefault();
        var objDTO = new List<NationalParkDTO>();
        objDTO.Add(_mapper.Map<NationalParkDTO>(obj));
        return Ok(objDTO);
    }
}
