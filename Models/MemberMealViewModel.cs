using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Models
{
    public class MemberMealViewModel
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }

        public string Bnumb { get; set; }
        public string Rank { get; set; }
        public DateTime Date { get; set; }
        public bool BreakFastEnabled { get; set; }
        public bool LunchEnabled { get; set; }
        public bool DinnerEnabled { get; set; }
        public bool TeaBreakEnabled { get; set; }
    }
}
