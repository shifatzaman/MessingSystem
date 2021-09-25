using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Models
{
    public class AddMemberMealViewModel
    {
        public DateTime Date { get; set; }

        public IList<MemberMealViewModel> MemberMeals { get; set; }
    }
}

