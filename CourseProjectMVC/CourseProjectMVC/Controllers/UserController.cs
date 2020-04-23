using System;
using System.Linq;
using System.Threading.Tasks;
using CourseProjectMVC.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _1stWebApp.utils.reflect;
using CourseProjectMVC.Models;
using NHibernate.Linq;

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
                string name = HttpContext.User.Identity.Name;
                var users = await _db.User.Where(x => x.UserName == name).ToArrayAsync();
                var user = users[0];
                if (!user.Orders.Contains(order)) return Unauthorized();
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


        [HttpPatch("mapStore")]
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

        [HttpPatch("patch")]
        [Authorize]
        public async Task<IActionResult> Patch([FromForm] UserModel model)
        {
            try
            {
                string name = HttpContext.User.Identity.Name;
                var users = await _db.User.Where(x => x.UserName == name).ToArrayAsync();
                var curUser = users[0];
                if (curUser == null) return BadRequest();
                Reflection.UpdateEntity(curUser, model);
                _db.User.Update(curUser);
                await _db.SaveChangesAsync();
                return Ok(curUser);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

        [HttpPatch("adminize/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PatchAdmin(string id)
        {
            try
            {
                var user = await _db.User.FindAsync(id);
                if (user == null || user.isActive) return BadRequest();
                user.isActive = true;
                await _userManager.RemoveFromRoleAsync(user, "Regular");
                await _userManager.AddToRoleAsync(user, "Admin");
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

        [HttpPatch("regularize/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PatchUser(string id)
        {
            try
            {
                var user = await _db.User.FindAsync(id);
                if (user == null || !user.isActive) return BadRequest();
                user.isActive = false;
                await _userManager.RemoveFromRoleAsync(user, "Admin");
                await _userManager.AddToRoleAsync(user, "Regular");
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

        [HttpDelete("deleteMyself")]
        [Authorize]
        public async Task<IActionResult> deleteMyself()
        {
            try
            {
                string name = HttpContext.User.Identity.Name;
                var users = await _db.User.Where(x => x.UserName == name).ToArrayAsync();
                var curUser = users[0];
                await _signInManager.SignOutAsync();
                var res = await _userManager.DeleteAsync(curUser);
                if (res.Succeeded) return Ok();
                return BadRequest(res.Errors);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

        [HttpDelete("deleteUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> deleteUser(string id)
        {
            try
            {
                var user = await _db.User.FindAsync(id);
                if (user == null) return BadRequest();
                await _userManager.UpdateSecurityStampAsync(user);
                var res = await _userManager.DeleteAsync(user);
                if (res.Succeeded) return Ok();
                return BadRequest(res.Errors);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }
    }
}