using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Domain
{
    public class DailyMessing
    {
        [Key]
        public int Id { get; set; }
        public int MealType { get; set; }
        public DateTime Date { get; set; }
    }
}
