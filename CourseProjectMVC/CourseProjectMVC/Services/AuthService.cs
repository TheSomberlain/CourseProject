using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseProjectMVC.Entities;
using CourseProjectMVC.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CourseProjectMVC.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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

