using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseProjectMVC.Interfaces;
using CourseProjectMVC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseProjectMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        private readonly IRedisService _redisService;

        public RedisController(IRedisService s)
        {
            _redisService = s;
        }

        [HttpGet("getByKey/{key}")]
        public async Task<IActionResult> Get(string key)
        {
            try
            {
                var item = await _redisService.Consume(key);
                return Ok(item);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm]string key, [FromForm] string value)
        {
            await _redisService.Put(key, value);
            return Ok();
        }
    }
}
