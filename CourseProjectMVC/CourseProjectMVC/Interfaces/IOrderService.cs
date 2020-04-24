using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseProjectMVC.Entities;
using CourseProjectMVC.Models;

namespace CourseProjectMVC.Interfaces
{
    public interface IOrderService
    {
        Task<dynamic> GetAll();
        Task<dynamic> GetById(int id);
        Task<Order> CreateOrder();
        Task<bool> DeleteOrder(int id);
        Task<Order> PatchOrder(int id, OrderModel model);
    }
}
