using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CourseProjectMVC.Entities
{
    public enum OrderStatus
    {
        Rendering,
        Checking,
        Ready
    }
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime ShippedDate { get; set; }
        [ForeignKey("StoreId")]
        public int? StoreId { get; set; }
        public Store Store { get; set; }
        [ForeignKey("Id")]
        [JsonIgnore]
        public string CustomerId { get; set; }
        public User Customer { get; set; }
        [JsonIgnore]
        public IEnumerable<OrderProduct> OrderProducts { get; set; }

    }
}
