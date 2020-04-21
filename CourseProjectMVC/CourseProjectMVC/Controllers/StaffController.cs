using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _1stWebApp.utils.reflect;
using CourseProjectMVC.Entities;
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
        private readonly MyDbContext _db;

        public StaffController(MyDbContext context)
        {
            _db = context;
        }
        // GET: api/Store
        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var Staff = await _db.Staff.AsNoTracking().OrderBy(x => x.StaffId).ToArrayAsync();
                return Ok(Staff);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(409);
            }
        }

        // GET: api/Store/5
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var Staff = await _db.Staff.FindAsync(id);
                if (Staff == null) return NotFound();
                return Ok(Staff);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

        // POST: api/Store
        [HttpPost("post")]
        public async Task<IActionResult> Post([FromBody] StaffModel model)
        {
            try
            {
                var Staff = new Staff();
                Reflection.UpdateEntity(Staff, model);
                await _db.AddAsync(Staff);
                await _db.SaveChangesAsync();
                return StatusCode(201, Staff);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

        // PUT: api/Store/5
        [HttpPatch("patch/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] StaffModel model)
        {
            try
            {
                var Staff = await _db.Staff.FindAsync(id);
                if (Staff == null) return BadRequest();
                Reflection.UpdateEntity(Staff, model);
                _db.Staff.Update(Staff);
                await _db.SaveChangesAsync();
                return Ok(Staff);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var Staff = await _db.Staff.FindAsync(id);
                if (Staff == null) return BadRequest();
                _db.Staff.Remove(Staff);
                await _db.SaveChangesAsync();
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
