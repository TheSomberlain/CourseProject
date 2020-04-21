using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CourseProjectMVC.Entities
{
    public class Store
    {
        [Key]
        public int StoreId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Building { get; set; }
        [ForeignKey("StockId")]
        [JsonIgnore]
        public int StockId { get; set; }
        public Stock Stock { get; set; } 
        [ForeignKey("AdminId")]
        public string AdminId { get; set; }
        public User Admin { get; set; }
        public IEnumerable<Staff> Staff { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}
