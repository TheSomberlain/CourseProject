using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseProjectMVC.Entities;
using CourseProjectMVC.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CourseProjectMVC.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly MyDbContext _db;
        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, MyDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }

        public async Task<User> GetCurrentUser(string name)
        {
            var user = await _db.User.Where(x => x.UserName == name).ToArrayAsync();
            var curUser = user[0];
            return curUser;
        }

        public async Task<SignInResult> Login(string name, string password)
        {
            var user = await _userManager.FindByNameAsync(name);
            if (user != null)
            {
                var res = await _signInManager.PasswordSignInAsync(user, password, false, false);
                return res;
            }
            throw new Exception("Login Failed");
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> Register(string name, string lastname, string password)
        {
            var user = new User() { UserName = name, LastName = lastname };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded) await _userManager.AddToRoleAsync(user, "Regular");
            return result;
        }
    }
}

