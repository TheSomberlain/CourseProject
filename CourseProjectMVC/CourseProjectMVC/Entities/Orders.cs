using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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
        public int StoreId { get; set; }
        public Store Store { get; set; }
        [ForeignKey("Id")]
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }
        public IEnumerable<OrderProduct> OrderProducts { get; set; }

    }
}
