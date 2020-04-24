using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _1stWebApp.utils.reflect;
using CourseProjectMVC.Entities;
using CourseProjectMVC.Models;
using CourseProjectMVC.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NHibernate.Util;

namespace CourseProjectMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IСurrencyService _сurrencyService;
        public ProductController(IProductService context, IСurrencyService s, IRedisService r)
        {
            _productService = context;
            _сurrencyService = s;
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get(string CountryCode)
        {
            try
            {
                double ratio = await _сurrencyService.GetCurrencyByKey(CountryCode);
                var products = await _productService.GetAll();
                var productsRatioed = products.Select(x =>
               {
                   x.Price /= ratio;
                   return x;
               });
                return Ok(productsRatioed);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(409);
            }
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(int id, string CountryCode)
        {
            try
            {
                double ratio = await _сurrencyService.GetCurrencyByKey(CountryCode);
                var product = await _productService.GetById(id);
                if (product == null) return NotFound();
                product.Price /= ratio;
                return Ok(product);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

        [HttpPost("post")]
        public async Task<IActionResult> Post([FromBody] ProductModel model)
        {
            try
            {
                var product = await _productService.CreateProduct(model);
                return StatusCode(201, product);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

        [HttpPatch("patch/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProductModel model)
        {
            try
            {
                var product = await _productService.PatchProduct(id, model);
                if (product == null) return BadRequest();
                return Ok(product);
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
                var res = await _productService.DeleteProduct(id);
                if (!res) return BadRequest();
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
