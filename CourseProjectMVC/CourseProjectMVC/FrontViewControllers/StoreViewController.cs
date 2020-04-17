using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using _1stWebApp.utils.reflect;
using CourseProjectMVC.Entities;
using CourseProjectMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseProjectMVC.Controllers
{
    [Route("store")]
    public class StoreViewController : Controller
    {
        private readonly MyDbContext _db;

        public StoreViewController(MyDbContext context)
        {
            _db = context;
        }
        // GET: api/Store
        [HttpGet("get")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var stores = await _db.Stores.AsNoTracking()
                    .OrderBy(x => x.StoreId).ToArrayAsync();
                return View(stores);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(409);
            }
        }

        // GET: api/Store/5
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var store = await _db.Stores.AsNoTracking()
                    .Include(x => x.Stock)
                    .Include(y => y.Orders)
                    .Include(z => z.Staff)
                    .FirstAsync(x => x.StoreId == id);
                if (store == null) return NotFound();
                return Ok(store);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(409);
            }
        }

    }
}
