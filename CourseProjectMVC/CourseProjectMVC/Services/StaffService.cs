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
    public class StaffService : IStaffService
    {
        private readonly MyDbContext _db;

        public StaffService(MyDbContext d)
        {
            _db = d;
        }
        public async Task<Staff> CreateStaff(StaffModel model)
        {
            var staff = new Staff();
            Reflection.UpdateEntity(staff, model);
            await _db.AddAsync(staff);
            await _db.SaveChangesAsync();
            return staff;
        }

        public async Task<bool> DeleteStaff(int id)
        {
            var staff = await _db.Staff.FindAsync(id);
            if (staff == null) return false;
            _db.Staff.Remove(staff);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Staff>> GetAll()
        {
            var staff = await _db.Staff.AsNoTracking().OrderBy(x => x.StaffId).ToArrayAsync();
            return staff;
        }

        public async Task<Staff> GetById(int id)
        {
            var staff = await _db.Staff.FindAsync(id);
            return staff;
        }

        public async Task<Staff> PatchStaff(int id, StaffModel model)
        {
            var staff = await _db.Staff.FindAsync(id);
            if (staff == null) return null;
            Reflection.UpdateEntity(staff, model);
            _db.Staff.Update(staff);
            await _db.SaveChangesAsync();
            return staff;
        }
    }
}
