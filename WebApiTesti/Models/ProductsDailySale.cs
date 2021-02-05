using System;
using System.Collections.Generic;

#nullable disable

namespace WebApiTesti.Models
{
    public partial class ProductsDailySale
    {
        public DateTime? OrderDate { get; set; }
        public string ProductName { get; set; }
        public double? DailySales { get; set; }
    }
}
