using MessingSystem.Data;
using MessingSystem.Domain;
using MessingSystem.Enums;
using MessingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Services
{
    public class MessingService : IMessingService
    {
        private readonly AppDbContext dbContext;
        public MessingService(AppDbContext appDbContext)
        {
            dbContext = appDbContext;
        }
        public void AddMemberMeal(MemberMeal memberMeal)
        {
            dbContext.MemberMeals.Add(memberMeal);
            dbContext.SaveChanges();
        }

        public void AddMessMember(MessMember member)
        {
            dbContext.MessMembers.Add(member);
            dbContext.SaveChanges();
        }

        public IList<MemberMeal> GetMemberMeals(int memberId)
        {
            return dbContext.MemberMeals.Where(m => m.MemberId == memberId).ToList() ;
        }

        public IList<MemberMealViewModel> GetMemberMealsByDate(DateTime date)
        {

            var mealsByDate = (from member in dbContext.MessMembers.Where(m => m.MemberStatus == (int)MemberStatus.Dining)
                               join memberMeals in dbContext.MemberMeals.Where(mm => mm.Date == date)
                               on member.Id equals memberMeals.MemberId
                               into memberMealsList
                               from mm in memberMealsList.DefaultIfEmpty()
                               select new MemberMealViewModel {
                                MemberId = member.Id,
                                MemberName = member.Name,
                                Bnumb = member.Bnumb,
                                Rank = member.Rank,
                                Date = date,
                                BreakFastEnabled = mm != null ? mm.BreakFastEnabled : true,
                                LunchEnabled = mm != null ? mm.LunchEnabled : true,
                                DinnerEnabled = mm != null ? mm.DinnerEnabled : true,
                                TeaBreakEnabled = mm != null ? mm.TeaBreakEnabled : true
                               });

            return mealsByDate.ToList();
        }

        public IList<MessMember> GetMessMembers(string searchString = null)
        {
            var members = dbContext.MessMembers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                members = members.Where(m => m.Name.Contains(searchString));
            }

            return members.ToList();
        }

        public void AddMemberMeals(IList<MemberMealViewModel> models)
        {
            var memberMeals = models.Select(m => new MemberMeal
            {
                Date = m.Date,
                MemberId = m.MemberId,
                BreakFastEnabled = m.BreakFastEnabled,
                LunchEnabled = m.LunchEnabled,
                DinnerEnabled = m.DinnerEnabled,
                TeaBreakEnabled = m.TeaBreakEnabled
            }).ToList();
            dbContext.MemberMeals.AddRange(memberMeals);
            dbContext.SaveChanges();
        }

        public void DeleteAllMeals(DateTime date)
        {
            var meals = dbContext.MemberMeals.Where(m => m.Date == date).ToList();
            dbContext.MemberMeals.RemoveRange(meals);
        }

        public void AddDailyMessing(AddDailyMessingViewModel model)
        {
            var dailyMessing = new DailyMessing
            {
                MealType = model.MealType,
                Date = model.Date
            };

            dbContext.DailyMessings.Add(dailyMessing);
            dbContext.SaveChanges();

            if (dailyMessing.Id > 0)
            {
                if (model.DailyMessingItems.Any())
                {
                    var messingItems = model.DailyMessingItems.Select(m => new DailyMessingItem
                    {
                        DailyMessingId = dailyMessing.Id,
                        ItemType = m.ItemType,
                        Quantity = m.Quantity
                    });

                    var dailyMessingItemTypeList = messingItems.Select(m => m.ItemType).ToList();

                    dbContext.DailyMessingItems.AddRange(messingItems);
                    dbContext.SaveChanges();

                    //Deduct from inventory
                    var invetoryItemTypes = dbContext.InventoryItemTypes.Where(it => dailyMessingItemTypeList.Contains(it.ItemTypeId)).ToList();

                    foreach (var item in invetoryItemTypes)
                    {
                        item.Quantity -= messingItems.Where(mi => mi.ItemType == item.ItemTypeId).Select(mi => mi.Quantity).Sum();
                    }

                    dbContext.InventoryItemTypes.UpdateRange(invetoryItemTypes);
                    dbContext.SaveChanges();
                }
            }
        }

        public IList<FetchDailyMessingViewModel> GetDailyMessings(DateTime date)
        {
            var dailyMessings = dbContext.DailyMessings.Where(d => d.Date == date);

            var messingWithItems = (from dailyMessing in dailyMessings
                                   join messingItems in dbContext.DailyMessingItems
                                   on dailyMessing.Id equals messingItems.DailyMessingId
                                   join itemType in dbContext.InventoryItemTypes
                                   on messingItems.ItemType equals itemType.ItemTypeId
                                   select new FlatDailyMessingViewModel
                                   {
                                       Id = dailyMessing.Id,
                                       MealType = dailyMessing.MealType,
                                       Date = dailyMessing.Date,
                                       ItemType = messingItems.ItemType,
                                       ItemName = itemType.Name,
                                       Unit = itemType.Unit,
                                       UnitPrice = itemType.UnitPrice,
                                       Quantity = messingItems.Quantity
                                   }).ToList();

            var membersAvailableForMeals = GetMembersAvailableForMeal(date);

            var uniqueMessingIds = messingWithItems.Select(m => m.Id).Distinct();

            var result = new List<FetchDailyMessingViewModel>();

            foreach (var id in uniqueMessingIds)
            {

                var itemsVMs = messingWithItems.Where(mi => mi.Id == id).Select(mi => new DailyMessingItemViewModel
                {
                    DailyMessingId = mi.Id,
                    ItemType = mi.ItemType,
                    ItemName = mi.ItemName,
                    Unit = mi.Unit,
                    UnitPrice = mi.UnitPrice,
                    Quantity = mi.Quantity
                }).ToList();

                var mealType = messingWithItems.Where(mi => mi.Id == id).Select(mi => mi.MealType).FirstOrDefault();
                var memberCount = 0;
                if (Enum.IsDefined(typeof(MealTypes), mealType))
                {
                    membersAvailableForMeals.TryGetValue(mealType, out memberCount);
                }

                var dailyMessingVM = new FetchDailyMessingViewModel
                {
                    Id = id,
                    Date = date,
                    MealType = mealType,
                    DailyMessingItems = itemsVMs,
                    Members = memberCount
                };

                result.Add(dailyMessingVM);
            }

            return result;
        }

        private Dictionary<int, int> GetMembersAvailableForMeal(DateTime date)
        {
            var memberMeals = GetMemberMealsByDate(date);
            var mealTypeWithMemberNumberPair = new Dictionary<int, int>();

            var breakFastEnabledMembers = memberMeals.Where(m => m.BreakFastEnabled).Select(m => m.MemberId).Count();
            mealTypeWithMemberNumberPair.Add((int)MealTypes.BreakFast, breakFastEnabledMembers);

            var lunchEnabledMembers = memberMeals.Where(m => m.LunchEnabled).Select(m => m.MemberId).Count();
            mealTypeWithMemberNumberPair.Add((int)MealTypes.Lunch, lunchEnabledMembers);

            var dinnerEnabledMembers = memberMeals.Where(m => m.DinnerEnabled).Select(m => m.MemberId).Count();
            mealTypeWithMemberNumberPair.Add((int)MealTypes.Dinner, dinnerEnabledMembers);

            var teaBreakEnabledMembers = memberMeals.Where(m => m.TeaBreakEnabled).Select(m => m.MemberId).Count();
            mealTypeWithMemberNumberPair.Add((int)MealTypes.TeaBreak, teaBreakEnabledMembers);

            return mealTypeWithMemberNumberPair;
        }
    }
}
