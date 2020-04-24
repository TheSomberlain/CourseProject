using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseProjectMVC.Entities;
using CourseProjectMVC.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseProjectMVC.Services
{
    public class StockServices : IStockService
    {
        private readonly MyDbContext _db;
        public StockServices(MyDbContext s)
        {
            _db = s;
        }
        public async Task<IEnumerable<Stock>> GetAll()
        {

            var stocks = await _db.Stock.AsNoTracking().OrderBy(x => x.StockId).ToArrayAsync();
            return stocks;
        }

        public async Task<Stock> GetById(int id)
        {
            var stock = await _db.Stock.FindAsync(id);
            return stock;
        }

        public async Task<Stock> PatchStock(int id, int productId)
        {
            var product = await _db.Product.FindAsync(productId);
            var stock = await _db.Stock.FindAsync(id);
            if (stock == null || product == null) return null;
            if (stock.Products.ContainsKey(product.Name))
            {
                stock.Products[product.Name]++;
            }
            else
            {
                stock.Products.Add(product.Name, productId);
            }

            _db.Stock.Update(stock);
            await _db.SaveChangesAsync();
            return stock;
        }
    }
}
