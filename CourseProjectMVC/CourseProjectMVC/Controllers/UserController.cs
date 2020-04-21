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
using Newtonsoft.Json;
using System.Web;

namespace CourseProjectMVC.Controllers
{

    [ApiController]
    [Route("api/user")]
    public class CustomerServiceController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly MyDbContext _db;
        public CustomerServiceController(UserManager<User> um, SignInManager<User> signInManager, MyDbContext context)
        {
            _userManager = um;
            _signInManager = signInManager;
            _db = context;
        }

        [HttpGet("get")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCustomers()
        {
            var customers = await _db.User.AsNoTracking()
                .OrderBy(x => x.Id).ToArrayAsync();
            return Ok(customers);
        }

        [HttpGet("get/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCustomer(string id)
        {
            var customers = await _db.User.AsNoTracking()
                .Where(z => z.Id == id)
                .OrderBy(x => x.Id).ToArrayAsync();
            return Ok(customers);
        }

        [Authorize]
        [HttpPost("newOrder")]
        public async Task<IActionResult> CreateOrder()
        {
            try
            {
                string name = HttpContext.User.Identity.Name;
                var user = await _db.User.Where(x => x.UserName == name).ToArrayAsync();
                var Order = new Order
                {
                    OrderStatus = OrderStatus.Rendering,
                    OrderDate = DateTime.Now,
                    CustomerId = user[0].Id
                };
                await _db.AddAsync(Order);
                await _db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

        [Authorize]
        [HttpPatch("addProduct")]
        public async Task<IActionResult> AddProduct([FromForm]int OrderId, [FromForm] int ProductId)
        {
            try
            {
                var order = await _db.Orders.FindAsync(OrderId);
                var product = await _db.Product.FindAsync(ProductId);
                if (order == null || product == null) return BadRequest();
                await _db.OrderProduct.AddAsync(new OrderProduct { OrderId = order.OrderId, ProductId = product.ProductId });
                await _db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

        [HttpPatch("assignAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignAdmin(int storeId)
        {
            try
            {
                string name = HttpContext.User.Identity.Name;
                var user = await _db.User.Where(x => x.UserName == name).ToArrayAsync();
                var store = await _db.Stores.FindAsync(storeId);
                if (store == null || store.AdminId != null) return BadRequest();
                store.AdminId = user[0].Id;
                 _db.Update(store);
                await _db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }


        [HttpPatch("assignAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MapStores(int storeId)
        {
            try
            {
                //TODO
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