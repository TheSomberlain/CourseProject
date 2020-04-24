using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CourseProjectMVC.Interfaces
{
    public interface IAuthService
    {
        Task<IdentityResult> Register(string name, string lastname, string password);
        Task<SignInResult> Login(string name, string password);
        Task Logout();
    }
}
