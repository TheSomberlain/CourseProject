using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CourseProjectMVC;
using CourseProjectMVC.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using  Newtonsoft.Json;
using System.Web;
using CourseProjectMVC.Interfaces;
using Microsoft.AspNetCore.Authentication;

namespace CourseProjectMVC.Controllers
{
    
    [ApiController]
    [Route("api/user")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService service)
        {
            _authService = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] string name,
            [FromForm] string lastName,
            [FromForm] string password)
        {
            var result = await _authService.Register(name, lastName, password);   
            if (result.Succeeded) return Ok();
            return StatusCode(409);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] string name, [FromForm] string password)
        {
            try
            {
                var res = await _authService.Login(name, password);
                if (res.Succeeded) return Ok();
                return StatusCode(405);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            return Ok();
        }
    }
}