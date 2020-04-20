using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProjectMVC.Entities
{
    public class Stock
    {
        public int StockId { get; set; }
        public Store Store { get; set; }
        public Dictionary<string, int> Products { get; set; }

    }
}
