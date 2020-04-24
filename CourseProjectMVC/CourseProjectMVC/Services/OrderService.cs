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
    public class OrderService : IOrderService
    {
        private readonly MyDbContext _db;

        public OrderService(MyDbContext context)
        {
            _db = context;
        }
        public async Task<Order> CreateOrder()
        {
            var order = new Order
            {
                OrderStatus = OrderStatus.Rendering,
                OrderDate = DateTime.Now
            };
            await _db.AddAsync(order);
            await _db.SaveChangesAsync();
            return order;
        }

        public async Task<bool> DeleteOrder(int id)
        {

            var order = await _db.Orders.FindAsync(id);
            if (order == null) return false;
            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<dynamic> GetAll()
        {

            var order = await _db.Orders.AsNoTracking()
                .Include(x => x.Customer)
                .Select(x => new
                {
                    x.OrderId,
                    x.OrderDate,
                    x.OrderStatus,
                    x.RequiredDate,
                    x.ShippedDate,
                    x.Store,
                    Customer = new { x.Customer.FistName, x.Customer.LastName }
                })
                .ToArrayAsync();
            return order;
        }

        public async Task<dynamic> GetById(int id)
        {
            var order = await _db.Orders.AsNoTracking()
                .Include(x => x.Customer)
                .Select(x => new
                {
                    x.OrderId,
                    x.OrderDate,
                    x.OrderStatus,
                    x.RequiredDate,
                    x.ShippedDate,
                    x.Store,
                    Customer = new { x.Customer.FistName, x.Customer.LastName }
                })
                .FirstOrDefaultAsync(z => z.OrderId == id);
            return order;
        }

        public async Task<Order> PatchOrder(int id, OrderModel model)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order == null) return null;
            Reflection.UpdateEntity(order, model);
            _db.Orders.Update(order);
            await _db.SaveChangesAsync();
            return order;
        }
    }
}
