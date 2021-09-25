using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Models
{
    public class FlatDailyMessingViewModel
    {
        public int Id { get; set; }
        public int MealType { get; set; }

        public DateTime Date { get; set; }
        public string MealName { get; set; }

        public int ItemType { get; set; }
        public string ItemName { get; set; }

        public string Unit { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Quantity { get; set; }

        public decimal ItemTotalPrice { get; set; }

        public decimal TotalPrice { get; set; }

    }
}
