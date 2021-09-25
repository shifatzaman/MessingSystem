using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Domain
{
    public class MealItem
    {
        [Key]
        public int Id { get; set; }

        public int ItemType { get; set; }

        public decimal Quantity { get; set; }
    }
}
