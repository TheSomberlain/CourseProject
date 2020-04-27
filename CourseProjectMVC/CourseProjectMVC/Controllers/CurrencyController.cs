using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseProjectMVC.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseProjectMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CurrencyController : ControllerBase
    {
        private readonly IСurrencyService _currencyService;
        private readonly IRedisService _redisService;

        public CurrencyController(IСurrencyService s, IRedisService r)
        {
            _currencyService = s;
            _redisService = r;
        }
        // GET: api/Currency
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var str=await _currencyService.GetCurrencies();
                return Ok(str);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

        // GET: api/Currency/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Currency
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            try
            {
                var currencies = await _currencyService.GetCurrencies();
                await _redisService.Put("currency", currencies);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

        // PUT: api/Currency/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
