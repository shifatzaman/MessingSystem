using MessingSystem.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Models
{
    public class FetchDailyMessingViewModel
    {
        public int Id { get; set; }
        public int MealType { get; set; }

        public DateTime Date { get; set; }
        public string MealName { 
            get 
            {
                try
                {
                    return Enum.GetName(typeof(MealTypes), MealType);
                }
                catch (Exception ex)
                {

                    return "N/A";
                }
                
            } 
        }

        public IList<DailyMessingItemViewModel> DailyMessingItems { get; set; }

        public decimal TotalPrice { 
            get
            { 
                if (DailyMessingItems != null)
                {
                    return DailyMessingItems.Select(di => di.TotalPrice).Sum();
                }

                return 0;
            } 
        }

        public int Members { get; set; }

        public decimal MealPricePerPerson
        {
            get
            {
                return Members > 0 ? Math.Ceiling(TotalPrice / (decimal)Members) : 0;
            }
        }
    }
}
