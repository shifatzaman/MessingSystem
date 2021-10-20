using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Models
{
    public class AddExtraMessingViewModel
    {
        public int Id { get; set; }

        public int MemberId { get; set; }

        public DateTime Date { get; set; }

        public int ItemType { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
