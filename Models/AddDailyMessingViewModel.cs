using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Models
{
    public class AddDailyMessingViewModel
    {
        public int Id { get; set; }
        public int MealType { get; set; }
        public DateTime Date { get; set; }
        public IList<DailyMessingItemViewModel> DailyMessingItems { get; set; }
    }
}
