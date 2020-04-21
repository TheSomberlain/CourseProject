using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CourseProjectMVC.Entities
{
    public class User : IdentityUser
    {
        public string FistName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Building { get; set; }
        public bool isActive { get; set; }
        public Store Store { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}
