using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Models
{
    public class FetchInventoryItemViewModel
    {
        public int InventoryId { get; set; }
        public DateTime? Date { get; set; }
        public int ItemType { get; set; }
        public decimal Quantity { get; set; }
        public string ItemName { get; set; }

        public string Unit { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice
        {
            get
            {
                return (decimal)Quantity * UnitPrice;
            }

        }

        public string QuantityWithUnit { 
            get
            {
                return string.Join(' ', new List<string>() { Quantity.ToString(), Unit });
            }
                
        }
    }
}
