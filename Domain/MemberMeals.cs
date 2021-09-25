using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Domain
{
    public class MemberMeal
    {
        [Key]
        public int Id { get; set; }
        public int MemberId { get; set; }
        public DateTime Date { get; set; }

        public bool BreakFastEnabled { get; set; }
        public bool LunchEnabled { get; set; }
        public bool DinnerEnabled { get; set; }
        public bool TeaBreakEnabled { get; set; }
    }
}
