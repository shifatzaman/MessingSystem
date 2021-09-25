using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Models
{
    public class DailyMessingItemViewModel
    {
        public int Id { get; set; }

        public int DailyMessingId { get; set; }
        public int ItemType { get; set; }
        public string ItemName { get; set; }

        public string Unit { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Quantity { get; set; }

        public decimal TotalPrice { 
            get 
            {
                return UnitPrice * Quantity;
            } 
        }

        public string QuantityWithUnit { 
            
            get
            {
                return string.Join(" ", new List<string>() { Quantity.ToString(), Unit });
            }
        }


    }
}
