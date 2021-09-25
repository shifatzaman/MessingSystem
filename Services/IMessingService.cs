using MessingSystem.Domain;
using MessingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Services
{
    public partial interface IMessingService
    {
        void AddMessMember(MessMember member);

        IList<MessMember> GetMessMembers(string searchString = null);

        void AddMemberMeal(MemberMeal memberMeal);

        IList<MemberMealViewModel> GetMemberMealsByDate(DateTime date);

        void AddMemberMeals(IList<MemberMealViewModel> models);

        void DeleteAllMeals(DateTime date);

        void AddDailyMessing(AddDailyMessingViewModel model);

        IList<FetchDailyMessingViewModel> GetDailyMessings(DateTime date);
    }
}
