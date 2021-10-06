using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Domain
{
    public class CafeterialBill
    {
        [Key]
        public int Id { get; set; }

        public int MemberId { get; set; }

        public DateTime Date { get; set; }

        public decimal Price { get; set; }
    }
}
