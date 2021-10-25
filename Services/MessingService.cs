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

        public void UpdateMessMember(MessMember member)
        {
            dbContext.MessMembers.Update(member);
            dbContext.SaveChanges();
        }

        public IList<MemberMeal> GetMemberMeals(int memberId)
        {
            return dbContext.MemberMeals.Where(m => m.MemberId == memberId).ToList() ;
        }

        public IList<MemberMealViewModel> GetMemberMealsByDate(DateTime date, int memberId = 0)
        {

            var mealsByDate = (from member in dbContext.MessMembers.Where(m => m.MemberStatus == (int)MemberStatus.Dining && (memberId == 0 || m.Id == memberId))
                               join memberMeals in dbContext.MemberMeals.Where(mm => mm.Date.Date == date)
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

        public IList<MemberMealViewModel> GetMemberMealsByDateRange(DateTime startDate, DateTime endDate)
        {

            var mealsByDate = (from member in dbContext.MessMembers.Where(m => m.MemberStatus == (int)MemberStatus.Dining)
                               join memberMeals in dbContext.MemberMeals.Where(mm => mm.Date.Date >= startDate && mm.Date.Date <= endDate)
                               on member.Id equals memberMeals.MemberId
                               into memberMealsList
                               from mm in memberMealsList.DefaultIfEmpty()
                               select new MemberMealViewModel
                               {
                                   MemberId = member.Id,
                                   MemberName = member.Name,
                                   Bnumb = member.Bnumb,
                                   Rank = member.Rank,
                                   Date = mm != null ? mm.Date.Date : DateTime.MinValue.Date,
                                   BreakFastEnabled = mm != null ? mm.BreakFastEnabled : true,
                                   LunchEnabled = mm != null ? mm.LunchEnabled : true,
                                   DinnerEnabled = mm != null ? mm.DinnerEnabled : true,
                                   TeaBreakEnabled = mm != null ? mm.TeaBreakEnabled : true
                               });

            return mealsByDate.ToList(); 
        }

        public MessMember GetMessMember(int memberId)
        {
            var members = dbContext.MessMembers.Where(m => m.Id == memberId).FirstOrDefault(); ;

            return members;
        }

        public IList<MessMemberViewModel> GetMessMembers(string searchString = null)
        {
            var members = dbContext.MessMembers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                members = members.Where(m => m.Name.Contains(searchString));
            }

            return (from member in members
                    join user in dbContext.Users
                    on member.UserId equals user.UserId
                    into memberUser
                    from mu in memberUser.DefaultIfEmpty()
                    select new MessMemberViewModel
                    {
                        Id  = member.Id,

                        Name = member.Name,

                        Bnumb = member.Bnumb,

                        Rank = member.Rank,

                        Appt = member.Appt,

                        Unit = member.Unit,
                        ContactNo  = member.ContactNo,
                        RoomNo = member.RoomNo,
                        DateOfEntry = member.DateOfEntry,
                        MaritialStatus = member.MaritialStatus,
                        MemberStatus = member.MemberStatus,
                        UserId = member.UserId,
                        Email = mu != null ? mu.Email : string.Empty
                    }).ToList();
        }

        public MessMemberViewModel GetMessMemberById(int memberId)
        {
            var members = dbContext.MessMembers.Where(mm => mm.Id == memberId).AsQueryable();


            return (from member in members
                    join user in dbContext.Users
                    on member.UserId equals user.UserId
                    into memberUser
                    from mu in memberUser.DefaultIfEmpty()
                    select new MessMemberViewModel
                    {
                        Id = member.Id,

                        Name = member.Name,

                        Bnumb = member.Bnumb,

                        Rank = member.Rank,

                        Appt = member.Appt,

                        Unit = member.Unit,
                        ContactNo = member.ContactNo,
                        RoomNo = member.RoomNo,
                        DateOfEntry = member.DateOfEntry,
                        MaritialStatus = member.MaritialStatus,
                        MemberStatus = member.MemberStatus,
                        UserId = member.UserId,
                        Email = mu != null ? mu.Email : string.Empty
                    }).FirstOrDefault();
        }

        public void AddMemberMeals(IList<MemberMealViewModel> models)
        {
            var memberMeals = models.Select(m => new MemberMeal
            {
                Date = m.Date.Date,
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
            var meals = dbContext.MemberMeals.Where(m => m.Date.Date == date.Date).ToList();
            dbContext.MemberMeals.RemoveRange(meals);
        }
        public void AddDailyMessingTemplate(DailyMessingTemplateViewModel model)
        {
            var template = new DailyMessingTemplate
            {
                MealType = model.MealType,
                TemplateName = model.TemplateName
            };

            dbContext.DailyMessingTemplates.Add(template);
            dbContext.SaveChanges();

            if (template.Id > 0)
            {
                if (model.DailyMessingTemplateItems.Any())
                {
                    var messingTemplateItems = model.DailyMessingTemplateItems.Select(m => new DailyMessingTemplateItem
                    {
                        DailyMessingTemplateId = template.Id,
                        ItemType = m.ItemType,
                        Quantity = m.Quantity
                    });

                    dbContext.DailyMessingTemplateItems.AddRange(messingTemplateItems);
                    dbContext.SaveChanges();
                }
            }
        }

        public IList<DailyMessingTemplate> GetDailyMessingTemplates()
        {
            var dailyMessingTemplates = dbContext.DailyMessingTemplates.ToList();

            if (dailyMessingTemplates != null)
            {
                var templateIdList = dailyMessingTemplates.Select(dt => dt.Id).ToList();

                var templateItemList = dbContext.DailyMessingTemplateItems.Where(it => templateIdList.Contains(it.DailyMessingTemplateId))
                                                                          .ToList();

                foreach (var item in dailyMessingTemplates)
                {
                    item.DailyMessingTemplateItems = templateItemList.Where(it => it.DailyMessingTemplateId == item.Id).ToList();
                }

            }

            return dailyMessingTemplates;
        }

        public void AddDailyMessing(AddDailyMessingViewModel model)
        {
            var dailyMessing = new DailyMessing
            {
                MealType = model.MealType,
                Date = model.Date.Date
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
            var dailyMessings = dbContext.DailyMessings.Where(d => d.Date.Date == date.Date);

            var messingWithItems = (from dailyMessing in dailyMessings
                                   join messingItems in dbContext.DailyMessingItems
                                   on dailyMessing.Id equals messingItems.DailyMessingId
                                   join itemType in dbContext.InventoryItemTypes
                                   on messingItems.ItemType equals itemType.ItemTypeId
                                   select new FlatDailyMessingViewModel
                                   {
                                       Id = dailyMessing.Id,
                                       MealType = dailyMessing.MealType,
                                       Date = dailyMessing.Date.Date,
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
                    Date = date.Date,
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

        private IList<MealActiveInfo> GetMembersAvailableForMealByDateRange(DateTime startDate, DateTime endDate)
        {
            var memberMeals = GetMemberMealsByDateRange(startDate, endDate);

            var diningMemberCount = dbContext.MessMembers.Where(m => m.MemberStatus == (int)MemberStatus.Dining).Count();

            var selectedDate = startDate;

            var memberAvailableForMealByDate = new List<MealActiveInfo>();

            while(selectedDate <= endDate)
            {
                
                var mealsOnDate = memberMeals.Where(m => m.Date.Date == selectedDate.Date).ToList();

                if (mealsOnDate != null && mealsOnDate.Count > 0)
                {
                    var mealTypeWithMemberNumberPair = new Dictionary<int, int>();

                    var breakFastEnabledMembers = diningMemberCount -  mealsOnDate.Where(m => !m.BreakFastEnabled).Select(m => m.MemberId).Count();
                    mealTypeWithMemberNumberPair.Add((int)MealTypes.BreakFast, breakFastEnabledMembers);

                    var lunchEnabledMembers = diningMemberCount - mealsOnDate.Where(m => !m.LunchEnabled).Select(m => m.MemberId).Count();
                    mealTypeWithMemberNumberPair.Add((int)MealTypes.Lunch, lunchEnabledMembers);

                    var dinnerEnabledMembers = diningMemberCount - mealsOnDate.Where(m => !m.DinnerEnabled).Select(m => m.MemberId).Count();
                    mealTypeWithMemberNumberPair.Add((int)MealTypes.Dinner, dinnerEnabledMembers);

                    var teaBreakEnabledMembers = diningMemberCount - mealsOnDate.Where(m => !m.TeaBreakEnabled).Select(m => m.MemberId).Count();
                    mealTypeWithMemberNumberPair.Add((int)MealTypes.TeaBreak, teaBreakEnabledMembers);

                    var item = new MealActiveInfo
                    {
                        Date = selectedDate.Date,
                        MealActivationKeyValuePair = mealTypeWithMemberNumberPair
                    };

                    memberAvailableForMealByDate.Add(item);
                }
                else
                {
                    var mealTypeWithMemberNumberPair = new Dictionary<int, int>();
                    mealTypeWithMemberNumberPair.Add((int)MealTypes.BreakFast, diningMemberCount);
                    mealTypeWithMemberNumberPair.Add((int)MealTypes.Lunch, diningMemberCount);
                    mealTypeWithMemberNumberPair.Add((int)MealTypes.Dinner, diningMemberCount);
                    mealTypeWithMemberNumberPair.Add((int)MealTypes.TeaBreak, diningMemberCount);
                    var item = new MealActiveInfo
                    {
                        Date = selectedDate.Date,
                        MealActivationKeyValuePair = mealTypeWithMemberNumberPair
                    };

                    memberAvailableForMealByDate.Add(item);
                }

                selectedDate = selectedDate.AddDays(1);
            }

            return memberAvailableForMealByDate;
        }

        public void AddExtraMessing(ExtraMessing extraMessing)
        {
            dbContext.ExtraMessings.Add(extraMessing);
            dbContext.SaveChanges();

            if (extraMessing.Id > 0)
            {
                var inventoryItem = dbContext.InventoryItemTypes.Where(it => it.ItemTypeId == extraMessing.ItemType).FirstOrDefault();

                if (inventoryItem != null)
                {
                    inventoryItem.Quantity -= extraMessing.Quantity;
                    dbContext.InventoryItemTypes.Update(inventoryItem);
                    dbContext.SaveChanges();
                }
            }
        }

        public IList<ExtraMessingViewModel> GetExtraMessings(int memberId)
        {
            return (from em in dbContext.ExtraMessings.Where(m => m.MemberId == memberId)
                    join it in dbContext.InventoryItemTypes
                    on em.ItemType equals it.ItemTypeId
                    select new ExtraMessingViewModel
                    {
                        Id = em.Id,
                        MemberId = em.MemberId,
                        Date = em.Date,
                        ItemType = em.ItemType,
                        ItemName = it.Name,
                        Unit = it.Unit,
                        UnitPrice = it.UnitPrice,
                        Quantity = em.Quantity
                    }).ToList();
        }

        public void DeleteExtraMessing(int extraMessingId)
        {
            var extraMessing = dbContext.ExtraMessings.Where(e => e.Id == extraMessingId).FirstOrDefault();

            if (extraMessing != null)
            {
                dbContext.ExtraMessings.Remove(extraMessing);
                dbContext.SaveChanges();

                if (extraMessing.ItemType > 0)
                {
                    var inventoryItem = dbContext.InventoryItemTypes.Where(it => it.ItemTypeId == extraMessing.ItemType).FirstOrDefault();

                    if (inventoryItem != null)
                    {
                        inventoryItem.Quantity += extraMessing.Quantity;
                        dbContext.InventoryItemTypes.Update(inventoryItem);
                        dbContext.SaveChanges();
                    }
                }
            }
        }

        public void AddCafeterialBill(CafeterialBill cafeterialBill)
        {
            if (cafeterialBill.Id > 0)
            {
                dbContext.CafeterialBills.Update(cafeterialBill);
            }
            else
            {
                dbContext.CafeterialBills.Add(cafeterialBill);
            }

            dbContext.SaveChanges();
        }

        public IList<CafeterialBill> GetCafeterialBills(int memberId)
        {
            return dbContext.CafeterialBills.Where(e => e.MemberId == memberId).ToList();
        }

        public void DeleteCafeteriaBill(int cafeterialBillId)
        {
            var cafeBill = dbContext.CafeterialBills.Where(e => e.Id == cafeterialBillId).FirstOrDefault();

            if (cafeBill != null)
            {
                dbContext.CafeterialBills.Remove(cafeBill);
                dbContext.SaveChanges();
            }
        }

        public void AddUtilityBill(UtilityBill utilityBill)
        {
            if (utilityBill.Id > 0)
            {
                dbContext.UtilityBills.Update(utilityBill);
            }
            else
            {
                dbContext.UtilityBills.Add(utilityBill);
            }

            dbContext.SaveChanges();
        }

        public IList<UtilityBill> GetUtilityBills()
        {
            return dbContext.UtilityBills.ToList();
        }

        public void DeleteUtilityBill(int utilityBillId)
        {
            var utilityToDelete = dbContext.UtilityBills.Where(u => u.Id == utilityBillId).FirstOrDefault();
            
            if (utilityToDelete != null)
            {
                dbContext.UtilityBills.Remove(utilityToDelete);
                dbContext.SaveChanges();
            }
        }


        public MessBillViewModel GetMessBill(int memberId, DateTime startDate, DateTime endDate)
        {
            var dailyMessingsInDateRange = GetDailyMessingItemsByDateRange(startDate.Date, endDate.Date);

            var messMemberMealAvailability = dbContext.MemberMeals.Where(m => m.Date.Date >= startDate.Date && m.Date.Date <= endDate.Date && m.MemberId == memberId).ToList();

            var dailyBills = new List<DailyBillViewModel>();

            var selectedDate = startDate.Date;

            while (selectedDate <= endDate)
            {
                var dailyMessingOnDate = dailyMessingsInDateRange.Where(dm => dm.Date.Date == selectedDate.Date).ToList();
                var messMemberMealAvailabilityOnDate = messMemberMealAvailability.Where(m => m.Date.Date == selectedDate.Date).ToList();

                decimal breakfastBill = 0, lunchBill = 0, teaBreakBill = 0, dinnerBill = 0;

                if (dailyMessingOnDate != null)
                {
                    //breakfast
                    if ((messMemberMealAvailabilityOnDate == null || messMemberMealAvailabilityOnDate.Count == 0) || messMemberMealAvailabilityOnDate.Where(m => m.BreakFastEnabled).Any())
                    {
                        breakfastBill = dailyMessingOnDate.Where(dm => dm.MealType == (int)MealTypes.BreakFast).Select(dm => dm.MealPricePerPerson).Sum();
                    }

                    //lunch
                    if ((messMemberMealAvailabilityOnDate == null || messMemberMealAvailabilityOnDate.Count == 0) || messMemberMealAvailabilityOnDate.Where(m => m.LunchEnabled).Any())
                    {
                        lunchBill = dailyMessingOnDate.Where(dm => dm.MealType == (int)MealTypes.Lunch).Select(dm => dm.MealPricePerPerson).Sum();
                    }

                    //teabreak
                    if ((messMemberMealAvailabilityOnDate == null || messMemberMealAvailabilityOnDate.Count == 0) || messMemberMealAvailabilityOnDate.Where(m => m.TeaBreakEnabled).Any())
                    {
                        teaBreakBill = dailyMessingOnDate.Where(dm => dm.MealType == (int)MealTypes.TeaBreak).Select(dm => dm.MealPricePerPerson).Sum();
                    }

                    //dinner
                    if ((messMemberMealAvailabilityOnDate == null || messMemberMealAvailabilityOnDate.Count == 0) || messMemberMealAvailabilityOnDate.Where(m => m.DinnerEnabled).Any())
                    {
                        dinnerBill = dailyMessingOnDate.Where(dm => dm.MealType == (int)MealTypes.Dinner).Select(dm => dm.MealPricePerPerson).Sum();
                    }
                }

                var dailyBill = new DailyBillViewModel
                {
                     Date = selectedDate,
                     LunchBill = lunchBill,
                     BreakFastBill = breakfastBill,
                     DinnerBill = dinnerBill,
                     TeaBreakBill = teaBreakBill
                };

                dailyBills.Add(dailyBill);

                selectedDate = selectedDate.AddDays(1);
            }

            var extraMessingBill = GetTotalExtraMessingForMemberByDateRange(memberId, startDate, endDate);
            var cafeBill = GetTotalCafeBillForMemberByDateRange(memberId, startDate, endDate);
            var utilityBill = GetTotalUtilityBillForMemberByDateRange(startDate, endDate);

            var messBill = new MessBillViewModel
            {
                BillGenerationDate = DateTime.UtcNow.Date,
                BillPeriodStart = startDate,
                BillPeriodEnd = endDate,
                MemberId = memberId,
                DailyBills = dailyBills,
                TotalExtraMessing = Math.Ceiling(extraMessingBill),
                TotalCafeBill = Math.Ceiling(cafeBill),
                TotalUtilityBill = Math.Ceiling(utilityBill)
            };

            return messBill;

        }

        public IList<FetchDailyMessingViewModel> GetDailyMessingItemsByDateRange(DateTime startDate, DateTime endDate)
        {
            var dailyMessings = dbContext.DailyMessings.Where(d => d.Date.Date >= startDate && d.Date.Date <= endDate);

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

            var membersAvailableForMealsByDateRange = GetMembersAvailableForMealByDateRange(startDate, endDate);

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
                var date = messingWithItems.Where(mi => mi.Id == id).Select(mi => mi.Date.Date).FirstOrDefault();
                var memberCount = 0;
                var membersAvailableForMeals = membersAvailableForMealsByDateRange.Where(d => d.Date.Date == date).Select(d => d.MealActivationKeyValuePair).FirstOrDefault();


                if (Enum.IsDefined(typeof(MealTypes), mealType) && membersAvailableForMeals != null)
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

        public decimal GetTotalExtraMessingForMemberByDateRange(int memberId, DateTime startDate, DateTime endDate)
        {
            return dbContext.ExtraMessings.Where(e => e.MemberId == memberId && e.Date.Date >= startDate && e.Date.Date <= endDate).Select(e => e.Price).Sum();
        }

        public decimal GetTotalCafeBillForMemberByDateRange(int memberId, DateTime startDate, DateTime endDate)
        {
            return dbContext.CafeterialBills.Where(e => e.MemberId == memberId && e.Date.Date >= startDate && e.Date.Date <= endDate).Select(e => e.Price).Sum();
        }

        public decimal GetTotalUtilityBillForMemberByDateRange(DateTime startDate, DateTime endDate)
        {
            var totalUtilityBillInDateRange = dbContext.UtilityBills.Where(e => e.Date.Date >= startDate && e.Date.Date <= endDate).Select(e => e.Price).Sum();
            var memberCount = dbContext.MessMembers.Count();

            if (memberCount > 0)
            {
                return totalUtilityBillInDateRange / (decimal)memberCount;
            }

            return totalUtilityBillInDateRange;
        }

        public void DeleteMessMember(MessMember messMember)
        {
            dbContext.MessMembers.Remove(messMember);
            dbContext.SaveChanges();
        }

        public Room GetRoom(int roomId)
        {
            return dbContext.Rooms.Where(r => r.Id == roomId).FirstOrDefault();
        }

        public IList<Room> GetRooms(bool includeVacantRoomsOnly = false)
        {
            var rooms = dbContext.Rooms.AsQueryable();

            if (includeVacantRoomsOnly)
            {
                rooms = rooms.Where(r => !r.IsAllocated);
            }

            return rooms.ToList();
        }

        public void SaveRoom(Room room)
        {
            dbContext.Rooms.Add(room);
            dbContext.SaveChanges();
        }

        public void DeleteRoom(int roomId)
        {
            var room = GetRoom(roomId);

            if (room != null)
            {
                dbContext.Rooms.Remove(room);
                dbContext.SaveChanges();
            }
        }

        public void UpdateRoom(Room room)
        {
            dbContext.Rooms.Update(room);
            dbContext.SaveChanges();
        }
    }
}
