﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.DTOs;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
public class NationalParksController : ControllerBase
{
    private readonly INationalParkRepository _npRepoo;
    private readonly IMapper _mapper;

    public NationalParksController(INationalParkRepository npRepo, IMapper mapper)
    {
        _npRepoo = npRepo;
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
        var objList = _npRepoo.GetNationalParks();
        var objDTO = new List<NationalParkDTO>();
        foreach (var obj in objList)
        {
            objDTO.Add(_mapper.Map<NationalParkDTO>(obj));
        }
        return Ok(objDTO);
    }

    /// <summary>
    /// Get individual national park.
    /// </summary>
    /// <param name="nationalParkId">The Id of the National Park.</param>
    /// <returns></returns>
    [HttpGet("{nationalParkId:int}", Name = "GetNationalPark")]
    [ProducesResponseType(200, Type = typeof(NationalParkDTO))]
    [ProducesResponseType(404)]
    [ProducesDefaultResponseType]
    public IActionResult GetNationalPark(int nationalParkId)
    {
        var obj = _npRepoo.GetNationalPark(nationalParkId);

        if (obj == null)
            return NotFound();
        
        /* Auto-Mapper vs Nomarl Mappings----------------------- */
        var objDTO = _mapper.Map<NationalParkDTO>(obj);
        //var onjDTO = new NationalParkDTO
        //{
        //    Created = obj.Created,
        //    Id = obj.Id,
        //    Name = obj.Name,
        //    State = obj.State
        //};
        /* ---------------------------------------------------- */


        return Ok(objDTO);
    }

    /// <summary>
    /// Create individual National Park.
    /// </summary>
    /// <param name="nationalParkDTO"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(NationalParkDTO))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult CreateNationalPark([FromBody] NationalParkDTO nationalParkDTO)
    {
        if (nationalParkDTO == null)
            return BadRequest(ModelState);

        if (_npRepoo.NationalParkExists(nationalParkDTO.Name))
        {
            ModelState.AddModelError("", "National Park Exists!");
            return StatusCode(404, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDTO);

        if (!_npRepoo.CreateNationalPark(nationalParkObj))
        {
            ModelState.AddModelError("", $"Something went wrong when saving the record {nationalParkObj.Name}");
            return StatusCode(500, ModelState);
        }

        //return Ok();
        //return Ok(nationalParkObj);
        return CreatedAtRoute("GetNationalPark", new { nationalParkId = nationalParkObj.Id }, nationalParkObj);
    }

    [HttpPatch("{nationalParkId:int}", Name = "UpdateNationalPark")]
    [ProducesResponseType(204)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateNationalPark(int nationalParkId, [FromBody] NationalParkDTO nationalParkDTO)
    {
        if (nationalParkDTO == null || nationalParkId != nationalParkDTO.Id)
            return BadRequest(ModelState);

        var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDTO);

        if (!_npRepoo.UpdateNationalPark(nationalParkObj))
        {
            ModelState.AddModelError("", $"Something went wrong when updating the record {nationalParkObj.Name}");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    [HttpDelete("{nationalParkId:int}", Name = "DeleteNationalPark")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteNationalPark(int nationalParkId)
    {
        if (!_npRepoo.NationalParkExists(nationalParkId))
            return NotFound();

        var nationalParkObj = _npRepoo.GetNationalPark(nationalParkId);

        if (!_npRepoo.DeleteNationalPark(nationalParkObj))
        {
            ModelState.AddModelError("", $"Something went wrong when deleting the record {nationalParkObj.Name}");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
}