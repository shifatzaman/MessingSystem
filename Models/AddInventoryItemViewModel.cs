using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Models
{
    public class AddInventoryViewModel
    {
        public int InventoryId { get; set; }
        public DateTime? Date { get; set; }
        public int ItemType { get; set; }
        public decimal Quantity { get; set; }
    }
}
