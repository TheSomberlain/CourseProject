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
    public class StockController : ControllerBase
    {
        private readonly MyDbContext _db;

        public StockController(MyDbContext context)
        {
            _db = context;
        }
        // GET: api/Store
        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var stocks = await _db.Stock.AsNoTracking().OrderBy(x => x.StockId).ToArrayAsync();
                return Ok(stocks);
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
                var store = await _db.Stock.FindAsync(id);
                if (store == null) return NotFound();
                return Ok(store);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

        [HttpPatch("patch/{id}")]
        public async Task<IActionResult> Put(int id, Product product)
        {
            /*try
            {
                var stock = await _db.Stock.FindAsync(id);
                if (stock == null) return BadRequest();
                if (stock.Products.ContainsKey(product))
                {
                    stock.Products[product]++;
                }
                else
                {
                    stock.Products.Add(product, 1);
                }
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }*/
            return BadRequest();
        }
    }
}
