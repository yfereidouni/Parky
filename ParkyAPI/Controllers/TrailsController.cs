using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.DTOs;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers;

//[Route("api/[controller]")]
//[Route("api/Trails")]
[Route("api/v{version:apiVersion}/trails")]
[ApiController]
//[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecTrails")]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
public class TrailsController : ControllerBase
{
    private readonly ITrailRepository _trailRepoo;
    private readonly IMapper _mapper;

    public TrailsController(ITrailRepository trailRepoo, IMapper mapper)
    {
        _trailRepoo = trailRepoo;
        _mapper = mapper;
    }

    /// <summary>
    /// Get list of Trails.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(List<TrailDTO>))]
    public IActionResult GetTrails()
    {
        // Hint : We should NOT expose our domain model to MVC View.
        // So we send DTOs to the MVC View 
        var objList = _trailRepoo.GetTrails();
        var objDTO = new List<TrailDTO>();
        foreach (var obj in objList)
        {
            objDTO.Add(_mapper.Map<TrailDTO>(obj));
        }
        return Ok(objDTO);
    }

    /// <summary>
    /// Get individual Trail.
    /// </summary>
    /// <param name="trailId">The Id of the TRail.</param>
    /// <returns></returns>
    [HttpGet("{trailId:int}", Name = "GetTrail")]
    [ProducesResponseType(200, Type = typeof(TrailDTO))]
    [ProducesResponseType(404)]
    [ProducesDefaultResponseType]
    public IActionResult GetTrail(int trailId)
    {
        var obj = _trailRepoo.GetTrail(trailId);

        if (obj == null)
            return NotFound();

        var objDTO = _mapper.Map<TrailDTO>(obj);

        return Ok(objDTO);
    }

    [HttpGet("[action]/{nationalParkId:int}")]
    [ProducesResponseType(200, Type = typeof(TrailDTO))]
    [ProducesResponseType(404)]
    [ProducesDefaultResponseType]
    public IActionResult GetTrailsInNationalPark(int nationalParkId)
    {
        var objList = _trailRepoo.GetTrailsInNationalPark(nationalParkId);

        if (objList == null)
            return NotFound();

        var objDTO = new List<TrailDTO>();
        foreach (var obj in objList)
        {
            objDTO.Add(_mapper.Map<TrailDTO>(obj));
        }

        return Ok(objDTO);
    }

    /// <summary>
    /// Create individual Trail.
    /// </summary>
    /// <param name="trailDTO"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(TrailDTO))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult CreateTrail([FromBody] TrailCreateDTO trailCreateDTO)
    {
        if (trailCreateDTO == null)
            return BadRequest(ModelState);

        if (_trailRepoo.TrailExists(trailCreateDTO.Name))
        {
            ModelState.AddModelError("", "Trail Exists!");
            return StatusCode(404, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var trailObj = _mapper.Map<Trail>(trailCreateDTO);

        if (!_trailRepoo.CreateTrail(trailObj))
        {
            ModelState.AddModelError("", $"Something went wrong when saving the record {trailObj.Name}");
            return StatusCode(500, ModelState);
        }

        //return Ok();
        //return Ok(trailObj);
        return CreatedAtRoute("GetTrail", new { trailId = trailObj.Id }, trailObj);
    }

    [HttpPatch("{trailId:int}", Name = "UpdateTrail")]
    [ProducesResponseType(204)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateTrail(int trailId, [FromBody] TrailUpdateDTO trailUpdateDTO)
    {
        if (trailUpdateDTO == null || trailId != trailUpdateDTO.Id)
            return BadRequest(ModelState);

        var trailObj = _mapper.Map<Trail>(trailUpdateDTO);

        if (!_trailRepoo.UpdateTrail(trailObj))
        {
            ModelState.AddModelError("", $"Something went wrong when updating the record {trailObj.Name}");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    [HttpDelete("{trailId:int}", Name = "DeleteTrail")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteTrail(int trailId)
    {
        if (!_trailRepoo.TrailExists(trailId))
            return NotFound();

        var trailObj = _trailRepoo.GetTrail(trailId);

        if (!_trailRepoo.DeleteTrail(trailObj))
        {
            ModelState.AddModelError("", $"Something went wrong when deleting the record {trailObj.Name}");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
}
