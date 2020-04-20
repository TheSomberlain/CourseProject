using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CourseProjectMVC.Controllers
{
    [ApiController]
    [Route("api/role")]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleController(RoleManager<IdentityRole> rm)
        {
            _roleManager = rm;
        }

        [HttpPost("post/{name}")]
        public async Task<IActionResult> CreateRole(string name)
        {
            bool x = await _roleManager.RoleExistsAsync("name");
            if (!x)
            {
                var role = new IdentityRole
                {
                    Name = name
                };
                await _roleManager.CreateAsync(role);
                return Ok();
            }

            return BadRequest();
        }
    }
}
