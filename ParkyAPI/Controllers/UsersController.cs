﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public UsersController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromBody] User model)
        {
            var user = _userRepo.Authenticate(model.Username, model.Password);
            if (user==null)
            {
                return BadRequest(new { message = "Username or Password is incorrect" });
            }
            return Ok(user);
        }
    }
}
