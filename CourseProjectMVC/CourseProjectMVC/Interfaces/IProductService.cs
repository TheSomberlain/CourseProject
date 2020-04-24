using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseProjectMVC.Entities;
using CourseProjectMVC.Models;

namespace CourseProjectMVC.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product> GetById(int id);
        Task<Product> CreateProduct(ProductModel model);
        Task<bool> DeleteProduct(int id);
        Task<Product> PatchProduct(int id, ProductModel model);
    }
}
