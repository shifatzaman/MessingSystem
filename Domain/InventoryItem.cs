using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Domain
{
    public class InventoryItem
    {
        public int InventoryItemId { get; set; }
        public DateTime? Date { get; set; }
        public int ItemType { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public bool IsDeleted { get; set; }

    }
}
