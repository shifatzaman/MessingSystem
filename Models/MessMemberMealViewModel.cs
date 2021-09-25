using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Models
{
    public class MessMemberMealViewModel
    {
        public int MemberId { get; set; }
        public DateTime Date { get; set; }

        public bool BreakFastEnabled { get; set; }
        public bool LunchEnabled { get; set; }
        public bool DinnerEnabled { get; set; }
        public bool TeaBreakEnabled { get; set; }
    }
}
