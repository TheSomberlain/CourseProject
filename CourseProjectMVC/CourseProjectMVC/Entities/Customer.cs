using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProjectMVC.Entities
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public string FistName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Building { get; set; }
        [ForeignKey("OrderId")]
        public int OrderId { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}
