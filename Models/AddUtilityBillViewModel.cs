using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Models
{
    public class AddUtilityBillViewModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Item { get; set; }

        public decimal Price { get; set; }
    }
}
