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
    [Authorize(Roles = "Admin")]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StockController(IStockService context)
        {
            _stockService = context;
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var stocks = await _stockService.GetAll();
                return Ok(stocks);
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
                var stock = await _stockService.GetById(id);
                if (stock == null) 
                    return NotFound();
                return Ok(stock);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }
        
    }
}
