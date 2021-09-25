using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Models
{
    public class AddInventoryItemTypeViewModel
    {
        public string Name { get; set; }
        public string Unit { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Quantity { get; set; }
    }
}
