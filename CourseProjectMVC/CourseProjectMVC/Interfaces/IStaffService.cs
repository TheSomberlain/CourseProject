using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseProjectMVC.Entities;
using CourseProjectMVC.Models;

namespace CourseProjectMVC.Interfaces
{
    public interface IStaffService
    {
        Task<IEnumerable<Staff>> GetAll();
        Task<Staff> GetById(int id);
        Task<Staff> CreateStaff(StaffModel model);
        Task<Staff> PatchStaff(int id, StaffModel model);
        Task<bool> DeleteStaff(int id);
    }
}
