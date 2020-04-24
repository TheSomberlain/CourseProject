using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _1stWebApp.utils.reflect;
using CourseProjectMVC.Entities;
using CourseProjectMVC.Interfaces;
using CourseProjectMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseProjectMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;

        public StaffController(IStaffService context)
        {
            _staffService = context;
        }
        // GET: api/Store
        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var staff = await _staffService.GetAll();
                return Ok(staff);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(409);
            }
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var staff = await _staffService.GetById(id);
                if (staff == null) return NotFound();
                return Ok(staff);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

        [HttpPost("post")]
        public async Task<IActionResult> Post([FromBody] StaffModel model)
        {
            try
            {
                var staff = await _staffService.CreateStaff(model);
                return StatusCode(201, staff);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

        [HttpPatch("patch/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] StaffModel model)
        {
            try
            {
                var staff = await _staffService.PatchStaff(id, model);
                if (staff == null)
                    return BadRequest();
                return Ok(staff);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var res = await _staffService.DeleteStaff(id);
                if (!res)
                    return BadRequest();
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

    }
}
