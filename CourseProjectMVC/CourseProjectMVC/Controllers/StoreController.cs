using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;
        private readonly IStockService _stockService;
        private readonly IStaffService _staffService;

        public StoreController(IStoreService context, IStockService service, IStaffService s)
        {
            _storeService = context;
            _stockService = service;
            _staffService = s;
        }
        
        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            
            try
            {
                var stores = await _storeService.GetAll();
               return Ok(stores);
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
                var store = await _storeService.GetById(id);
                if (store == null) return NotFound();
                return Ok(store);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("post")]
        public async Task<IActionResult> Post([FromBody] StoreModel model)
        {
            try
            {
                var store = await _storeService.CreateStore(model);
                return StatusCode(201, store);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("patch/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] StoreModel model)
        {
            try
            {
                var store = await _storeService.PatchStore(id, model);
                if (store == null)
                    return BadRequest();
                return Ok(store);
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
                var res = await _storeService.DeleteStore(id);
                if (!res)
                    return BadRequest();
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("addProduct/{id}")]
        public async Task<IActionResult> AddProduct(int id, [FromForm] int productId )
        {
            try
            {
                var store = await _storeService.GetById(id);
                if (store == null) return BadRequest();
                var res = await _stockService.PatchStock(store.StockId, productId);
                if (res == null) return BadRequest();
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("addStaff/{id}")]
        public async Task<IActionResult> AddStaff(int id, [FromForm] int staffId)
        {
            try
            {
                bool res1 = await _storeService.Contains(id);
                if (!res1) return BadRequest();
                var model = new StaffModel()
                {
                    StoreId = id
                };
                var res = await _staffService.PatchStaff(staffId,model);
                if (res == null) return BadRequest();
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return StatusCode(409);
            }
        }
    }
}
