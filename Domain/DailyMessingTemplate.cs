using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Domain
{
    public class DailyMessingTemplate
    {
        [Key]
        public int Id { get; set; }
        public string TemplateName { get; set; }
        public int MealType { get; set; }

        [NotMapped]
        public IList<DailyMessingTemplateItem> DailyMessingTemplateItems { get; set; }
    }

    public class DailyMessingTemplateItem
    {
        [Key]
        public int Id { get; set; }

        public int DailyMessingTemplateId { get; set; }
        public int ItemType { get; set; }

        public decimal Quantity { get; set; }
    }
}
