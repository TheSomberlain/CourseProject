using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseProjectMVC.Entities;
using CourseProjectMVC.Models;

namespace CourseProjectMVC.Interfaces
{
    public interface IStoreService
    {
        Task<IEnumerable<Store>> GetAll();
        Task<Store> GetById(int id);
        Task<Store> CreateStore(StoreModel model);
        Task<Store> PatchStore(int id, StoreModel model);
        Task<bool> DeleteStore(int id);
    }
}
