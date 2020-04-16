using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
                return Ok();
            }

            return StatusCode(409); ;
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

        [Authorize]
        [HttpPost("newOrder")]
        public async Task<IActionResult> CreateOrder(string customerId)
        {
            try
            {
                string name = HttpContext.User.Identity.Name;
                var user = await _db.Customer.Where(x=>x.UserName == name).ToArrayAsync();
                Console.WriteLine("name:" + name);
                var Order = new Order();
                Order.StoreId = 1;
                Order.OrderStatus = OrderStatus.Rendering;
                Order.OrderDate = DateTime.Now;
                Order.CustomerId = user[0].Id;
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

        [HttpPatch("addProduct")]
        public async Task<IActionResult> addProduct([FromForm]int OrderId, [FromForm] int ProductId)
        {
            try
            {
                var order = await _db.Orders.FindAsync(OrderId);
                var product = await _db.Product.FindAsync(ProductId);
                if (order == null || product == null) return BadRequest();
                await _db.OrderProduct.AddAsync(new OrderProduct {OrderId = order.OrderId, ProductId = product.ProductId});
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