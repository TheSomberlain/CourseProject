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
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IAuthService _authService;

        public OrderController(IOrderService context, IAuthService a)
        {
            _orderService = context;
            _authService = a;
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var order = await _orderService.GetAll();
                return Ok(order);
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
                var order = await _orderService.GetById(id);
                if (order == null) return NotFound();
                return Ok(order);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("post")]
        public async Task<IActionResult> Post()
        {
            try
            {
                var order = await _orderService.CreateOrder();
                return StatusCode(201, order);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("patch/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] OrderModel model)
        {
            try
            {
                var order = await _orderService.PatchOrder(id, model);
                if(order != null)
                    return Ok(order);
                return BadRequest();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var res = await _orderService.DeleteOrder(id);
                if (res) return Ok();
                return BadRequest();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

    }
}
