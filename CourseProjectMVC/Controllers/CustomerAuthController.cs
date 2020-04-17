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
using Microsoft.AspNetCore.Authentication;

namespace _1stWebApp.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;
        private readonly MyDbContext _db;
        public AuthController(UserManager<Customer> um, SignInManager<Customer> signInManager, MyDbContext context)
        {
            _userManager = um;
            _signInManager = signInManager;
            _db = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] string name,
            [FromForm] string lastName,
            [FromForm] string password)
        {
            var user = new Customer() { UserName = name, LastName = lastName };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Regular");
                return Ok();
            }
            return StatusCode(409);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] string name, [FromForm] string password)
        {
            var user = await _userManager.FindByNameAsync(name);
            if (user != null)
            {
                var res = await _signInManager.PasswordSignInAsync(user, password, false, false);
                if (res.Succeeded)
                {
                    return Ok();
                }
            }
            return StatusCode(409);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}