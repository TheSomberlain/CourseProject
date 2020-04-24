using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _1stWebApp.utils.reflect;
using CourseProjectMVC.Entities;
using CourseProjectMVC.Interfaces;
using CourseProjectMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseProjectMVC.Services
{
    public class ProductService : IProductService
    {
        private readonly MyDbContext _db;
        public ProductService(MyDbContext context)
        {
            _db = context;
        }
        public async Task<Product> CreateProduct(ProductModel model)
        {
            var product = new Product();
            Reflection.UpdateEntity(product, model);
            await _db.AddAsync(product);
            await _db.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _db.Product.FindAsync(id);
            if (product == null) return false;
            _db.Product.Remove(product);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            var products = await _db.Product.OrderBy(x => x.ProductId).ToArrayAsync();
            return products;
        }

        public async Task<Product> GetById(int id)
        {
            var product = await _db.Product.FindAsync(id);
            return product;
        }

        public async Task<Product> PatchProduct(int id, ProductModel model)
        {
            var product = await _db.Product.FindAsync(id);
            if (product == null) return null;
            Reflection.UpdateEntity(product, model);
            _db.Product.Update(product);
            await _db.SaveChangesAsync();
            return product;
        }
    }
}
