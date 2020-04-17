﻿using System;
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

namespace _1stWebApp.Controllers
{

    [ApiController]
    [Route("api/customer")]
    public class CustomerServiceController : ControllerBase
    {
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;
        private readonly MyDbContext _db;
        public CustomerServiceController(UserManager<Customer> um, SignInManager<Customer> signInManager, MyDbContext context)
        {
            _userManager = um;
            _signInManager = signInManager;
            _db = context;
        }

        [Authorize]
        [HttpPost("newOrder")]
        public async Task<IActionResult> CreateOrder(string customerId)
        {
            try
            {
                string name = HttpContext.User.Identity.Name;
                var user = await _db.Customer.Where(x => x.UserName == name).ToArrayAsync();
                Console.WriteLine("name:" + name);
                var Order = new Order
                {
                    StoreId = 1,
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
        public async Task<IActionResult> addProduct([FromForm]int OrderId, [FromForm] int ProductId)
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
    }
}