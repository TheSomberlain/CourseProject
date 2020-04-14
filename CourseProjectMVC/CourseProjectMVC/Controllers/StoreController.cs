using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    public class StoreController : ControllerBase
    {
        private readonly MyDbContext _db;

        public StoreController(MyDbContext context)
        {
            _db = context;
        }
        // GET: api/Store
        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            try
            {
               var stores = await _db.Stores.AsNoTracking().OrderBy(x => x.StoreId).ToArrayAsync();
               return Ok(stores);
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
                var store = await _db.Stores.FindAsync(id);
                if (store == null) return NotFound();
                return Ok(store);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

        // POST: api/Store
        [HttpPost("post")]
        public async Task<IActionResult> Post([FromBody] StoreModel model)
        {
            try
            {
                var store = new Store();
                Reflection.UpdateEntity(store,model);
                await _db.AddAsync(store);
                var stock = new Stock
                {
                    Store = store
                };
                stock.Products = new Dictionary<int, int>();
                await _db.AddAsync(stock);
                await _db.SaveChangesAsync();
                return StatusCode(201, store);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

        // PUT: api/Store/5
        [HttpPatch("patch/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] StoreModel model)
        {
            try
            {
                var store = await _db.Stores.FindAsync(id);
                if (store == null) return BadRequest();
                Reflection.UpdateEntity(store, model);
                _db.Stores.Update(store);
                await _db.SaveChangesAsync();
                return Ok(store);
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
                var store = await _db.Stores.FindAsync(id);
                if (store == null) return BadRequest();
                var stock = await _db.Stock.FindAsync(id);
                _db.Stores.Remove(store);
                _db.Stock.Remove(stock);
                await _db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

        [HttpPatch("addProduct/{id}")]
        public async Task<IActionResult> AddProduct(int id)
        {
            try
            {
                var product = await _db.Product.FindAsync(1);

                var stock = await _db.Stock.FindAsync(id);
                if (stock == null) return BadRequest();
                if (stock.Products.ContainsKey(product.ProductId))
                {
                    stock.Products[product.ProductId]++;
                }
                else
                {
                    stock.Products.Add(product.ProductId, 1);
                }

                _db.Stock.Update(stock);
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
