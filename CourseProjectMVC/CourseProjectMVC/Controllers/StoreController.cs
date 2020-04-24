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
    [Authorize(Roles = "Admin")]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;
        private readonly IStockService _stockService;

        public StoreController(IStoreService context, IStockService service)
        {
            _storeService = context;
            _stockService = service;
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

        [HttpPatch("addProduct/{id}")]
        public async Task<IActionResult> AddProduct(int id, [FromForm] int productId )
        {
            try
            {
                var res = await _stockService.PatchStock(id, productId);
                if (res == null) return BadRequest();
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
