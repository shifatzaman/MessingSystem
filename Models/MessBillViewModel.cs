using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Models
{
    public class MessBillViewModel
    {
        public int MemberId { get; set; }
        public DateTime BillGenerationDate { get; set; }
        public DateTime BillPeriodStart { get; set; }
        public DateTime BillPeriodEnd { get; set; }
        public IList<DailyBillViewModel> DailyBills { get; set; }


        public decimal TotalLunchBill {
            get
            {

                if (DailyBills != null && DailyBills.Count > 0)
                    return DailyBills.Select(d => d.LunchBill).Sum();

                return 0;
            }
        }
        public decimal TotalBreakFastBill {
            get
            {

                if (DailyBills != null && DailyBills.Count > 0)
                    return DailyBills.Select(d => d.BreakFastBill).Sum();

                return 0;
            }
        }
        public decimal TotalDinnerBill {
            get
            {

                if (DailyBills != null && DailyBills.Count > 0)
                    return DailyBills.Select(d => d.DinnerBill).Sum();

                return 0;
            }
        }
        public decimal TotalTeaBreakBill {
            get
            {

                if (DailyBills != null && DailyBills.Count > 0)
                    return DailyBills.Select(d => d.TeaBreakBill).Sum();

                return 0;
            }
        }


        public decimal TotalCasualBill { get {
                return TotalLunchBill + TotalBreakFastBill + TotalDinnerBill + TotalTeaBreakBill;
        } }
        public decimal TotalCafeBill { get; set; }
        public decimal TotalExtraMessing { get; set; }
        public decimal TotalUtilityBill { get; set; }

        public decimal TotalMessBill {
            get
            {
                return TotalCasualBill + TotalCafeBill + TotalExtraMessing + TotalUtilityBill;
            }
        }
    }

    public class DailyBillViewModel
    {
        public DateTime Date { get; set; }
        public decimal LunchBill { get; set; }
        public decimal BreakFastBill { get; set; }
        public decimal DinnerBill { get; set; }
        public decimal TeaBreakBill { get; set; }
    }

    public class MealActiveInfo
    {
        public DateTime Date { get; set; }

        public Dictionary<int, int> MealActivationKeyValuePair { get; set; }
    }
}
