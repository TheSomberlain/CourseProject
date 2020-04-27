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
    public class StoreService : IStoreService
    {
        private readonly MyDbContext _db;
        public StoreService(MyDbContext s)
        {
            _db = s;
        }
        public async Task<Store> CreateStore(StoreModel model)
        {
            var store = new Store();
            Reflection.UpdateEntity(store, model);
            await _db.AddAsync(store);
            var stock = new Stock
            {
                Store = store
            };
            stock.Products = new Dictionary<string, int>();
            await _db.AddAsync(stock);
            await _db.SaveChangesAsync();
            return store;
        }

        public async Task<bool> DeleteStore(int id)
        {
            var store = await _db.Stores.FindAsync(id);
            if (store == null) return false;
            var stock = await _db.Stock.FindAsync(store.StockId);
            _db.Stores.Remove(store);
            _db.Stock.Remove(stock);
            var staff1 =  await _db.Staff.Where(x => x.StoreId == id).ToArrayAsync();
            var staff = staff1.Select(x =>
            {
                x.StoreId = null;
                return x;
            });
            _db.Staff.UpdateRange(staff);    
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Store>> GetAll()
        {
            var stores = await _db.Stores.AsNoTracking()
                .Include(x => x.Stock)
                .Include(y => y.Orders)
                .Include(z => z.Staff)
                .OrderBy(x => x.StoreId).ToArrayAsync();
            return stores;
        }

        public async Task<Store> GetById(int id)
        {
            var store = await _db.Stores.AsNoTracking()
                .Include(x => x.Stock)
                .Include(y => y.Orders)
                .Include(z => z.Staff)
                .FirstOrDefaultAsync(x => x.StoreId == id);
            return store;
        }

        public async Task<Store> PatchStore(int id, StoreModel model)
        {
            var store = await _db.Stores.FindAsync(id);
            if (store == null) return null;
            Reflection.UpdateEntity(store, model);
            _db.Stores.Update(store);
            await _db.SaveChangesAsync();
            return store;
        }

        public async Task<bool> Contains(int id)
        {
            var res = await _db.Stores.Where(x => x.StoreId == id).FirstOrDefaultAsync();
            return res != null;
        }
    }
}
