using MessingSystem.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Models
{
    public class RoomViewModel
    {
        public int Id { get; set; }

        public int Type { get; set; }

        public string RoomNo { get; set; }

        public string AllocatedTo { get; set; }

        public bool IsAllocated { get; set; }

        public string RoomTypeName { 
            get {
                if (Enum.IsDefined(typeof(RoomTypes), Type))
                {
                    return string.Format("{0} Room", Enum.GetName(typeof(RoomTypes), Type));
                }

                return "Unknown";
            } 
        }

        public string Availability
        {
            get
            {
                return IsAllocated ? "Allocated" : "Vacant";
            }
        }
    }
}
