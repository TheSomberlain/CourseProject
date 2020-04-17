using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CourseProjectMVC.Entities
{
    public class Staff
    {
        public int StaffId { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey("StoreId")]
        [JsonIgnore]
        public int StoreId { get; set; }
        public Store Store { get; set; }
    }
}
