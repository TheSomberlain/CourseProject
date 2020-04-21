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
    public class OrderController : ControllerBase
    {
        private readonly MyDbContext _db;

        public OrderController(MyDbContext context)
        {
            _db = context;
        }
        // GET: api/Store
        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var Order = await _db.Orders.AsNoTracking()
                    .Include(x => x.Customer)
                    .Select(x => new
                        {x.OrderId, x.OrderDate, x.OrderStatus, x.RequiredDate, x.ShippedDate, x.Store,
                            Customer = new {x.Customer.FistName,x.Customer.LastName}})
                    .ToArrayAsync();
                
                return Ok(Order);
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
                var Order = await _db.Orders.AsNoTracking()
                    .Include(x => x.Customer)
                    .Select(x => new
                    {
                        x.OrderId, x.OrderDate, x.OrderStatus, x.RequiredDate, x.ShippedDate, x.Store,
                        Customer = new {x.Customer.FistName, x.Customer.LastName}
                    })
                    .FirstAsync(z => z.OrderId == id);
                if (Order == null) return NotFound();
                return Ok(Order);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

        // POST: api/Store
        [HttpPost("post")]
        public async Task<IActionResult> Post()
        {
            try
            {
                var Order = new Order
                {
                    OrderStatus = OrderStatus.Rendering,
                    OrderDate = DateTime.Now
                };
                await _db.AddAsync(Order);
                await _db.SaveChangesAsync();
                return StatusCode(201, Order);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

        // PUT: api/Store/5
        [HttpPatch("patch/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] OrderModel model)
        {
            try
            {
                var Order = await _db.Orders.FindAsync(id);
                if (Order == null) return BadRequest();
                Reflection.UpdateEntity(Order, model);
                _db.Orders.Update(Order);
                await _db.SaveChangesAsync();
                return Ok(Order);
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
                var Order = await _db.Orders.FindAsync(id);
                if (Order == null) return BadRequest();
                _db.Orders.Remove(Order);
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
