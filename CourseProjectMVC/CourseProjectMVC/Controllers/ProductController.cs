using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _1stWebApp.utils.reflect;
using CourseProjectMVC.Entities;
using CourseProjectMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseProjectMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly MyDbContext _db;

        public ProductController(MyDbContext context)
        {
            _db = context;
        }
        // GET: api/Store
        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var product = await _db.Product.AsNoTracking().OrderBy(x => x.ProductId).ToArrayAsync();
                return Ok(product);
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
                var product = await _db.Product.FindAsync(id);
                if (product == null) return NotFound();
                return Ok(product);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

        // POST: api/Store
        [HttpPost("post")]
        public async Task<IActionResult> Post([FromBody] ProductModel model)
        {
            try
            {
                var product = new Product();
                Reflection.UpdateEntity(product, model);
                await _db.AddAsync(product);
                await _db.SaveChangesAsync();
                return StatusCode(201, product);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

        // PUT: api/Store/5
        [HttpPatch("patch/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProductModel model)
        {
            try
            {
                var product = await _db.Product.FindAsync(id);
                if (product == null) return BadRequest();
                Reflection.UpdateEntity(product, model);
                _db.Product.Update(product);
                await _db.SaveChangesAsync();
                return Ok(product);
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
                var product = await _db.Product.FindAsync(id);
                if (product == null) return BadRequest();
                _db.Product.Remove(product);
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
