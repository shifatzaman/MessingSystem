using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Models
{
    public class DailyMessingTemplateViewModel
    {
        public int Id { get; set; }

        public string TemplateName { get; set; }
        public int MealType { get; set; }
        public IList<DailyMessingTemplateItemViewModel> DailyMessingTemplateItems { get; set; }
    }

    public class DailyMessingTemplateItemViewModel
    {
        public int ItemType { get; set; }
        public decimal Quantity { get; set; }
    }
}
