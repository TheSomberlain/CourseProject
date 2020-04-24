using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseProjectMVC.Entities;

namespace CourseProjectMVC.Interfaces
{
    public interface IStockService
    {
        Task<IEnumerable<Stock>> GetAll();
        Task<Stock> GetById(int id);
        Task<Stock> PatchStock(int id, int productId);
    }
}
