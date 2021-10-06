using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Models
{
    public class AddCafeteriaBillViewModel
    {
        public int Id { get; set; }

        public int MemberId { get; set; }

        public DateTime Date { get; set; }

        public decimal Price { get; set; }
    }
}
