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

        void UpdateMessMember(MessMember member);

        MessMemberViewModel GetMessMemberById(int memberId);

        IList<MessMemberViewModel> GetMessMembers(string searchString = null);
        MessMember GetMessMember(int memberId);

        void DeleteMessMember(MessMember messMember);

        void AddMemberMeal(MemberMeal memberMeal);

        IList<MemberMealViewModel> GetMemberMealsByDate(DateTime date, int memberId = 0);

        void AddMemberMeals(IList<MemberMealViewModel> models);

        void DeleteAllMeals(DateTime date);

        void AddDailyMessing(AddDailyMessingViewModel model);

        IList<FetchDailyMessingViewModel> GetDailyMessings(DateTime date);

        void AddExtraMessing(ExtraMessing extraMessing);

        IList<ExtraMessing> GetExtraMessings(int memberId);

        void DeleteExtraMessing(int extraMessingId);


        void AddCafeterialBill(CafeterialBill cafeterialBill);

        IList<CafeterialBill> GetCafeterialBills(int memberId);

        void DeleteCafeteriaBill(int cafeterialBillId);

        void AddUtilityBill(UtilityBill utilityBill);

        IList<UtilityBill> GetUtilityBills();

        void DeleteUtilityBill(int utilityBillId);

        MessBillViewModel GetMessBill(int memberId, DateTime startDate, DateTime endDate);

        Room GetRoom(int roomId);

        IList<Room> GetRooms(bool includeVacantRoomsOnly = false);

        void SaveRoom(Room room);

        void DeleteRoom(int roomId);

        void UpdateRoom(Room room);

    }
}
