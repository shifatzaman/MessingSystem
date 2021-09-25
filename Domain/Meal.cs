using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Domain
{
    public class Meal
    {
        [Key]
        public int Id { get; set; }
        public int Type { get; set; }

        public DateTime Date { get; set; }
    }
}
