using MessingSystem.Domain;
using MessingSystem.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Models
{
    public class RoomStatisticsViewModel
    {
        
        public RoomStatisticsViewModel(IList<Room> rooms)
        {
            Rooms = rooms;
        }

        private IList<Room> Rooms;

        public int GuestVacantRooms { get { 

                if (Rooms != null && Rooms.Count > 0)
                {
                    return Rooms.Where(r => r.Type == (int)RoomTypes.Guest && !r.IsAllocated).Count();
                }

                return 0;
            
         } }
        public int GuestAllocatedRooms {
            get
            {

                if (Rooms != null && Rooms.Count > 0)
                {
                    return Rooms.Where(r => r.Type == (int)RoomTypes.Guest && r.IsAllocated).Count();
                }

                return 0;

            }
        }

        public int TotalGuestRooms { get {
                return GuestVacantRooms + GuestAllocatedRooms;
        } }

        public int RegularVacantRooms {
            get
            {

                if (Rooms != null && Rooms.Count > 0)
                {
                    return Rooms.Where(r => r.Type == (int)RoomTypes.Regular && !r.IsAllocated).Count();
                }

                return 0;

            }
        }
        public int RegularAllocatedRooms {
            get
            {

                if (Rooms != null && Rooms.Count > 0)
                {
                    return Rooms.Where(r => r.Type == (int)RoomTypes.Regular && r.IsAllocated).Count();
                }

                return 0;

            }
        }

        public int TotalRegularRooms
        {
            get
            {
                return RegularVacantRooms + RegularAllocatedRooms;
            }
        }

        public int VipVacantRooms {
            get
            {

                if (Rooms != null && Rooms.Count > 0)
                {
                    return Rooms.Where(r => r.Type == (int)RoomTypes.Vip && !r.IsAllocated).Count();
                }

                return 0;

            }
        }
        public int VipAllocatedRooms {
            get
            {

                if (Rooms != null && Rooms.Count > 0)
                {
                    return Rooms.Where(r => r.Type == (int)RoomTypes.Vip && r.IsAllocated).Count();
                }

                return 0;

            }
        }

        public int TotalVipRooms
        {
            get
            {
                return VipVacantRooms + VipAllocatedRooms;
            }
        }

        public int TotalVacantRooms
        {
            get
            {
                return RegularVacantRooms + GuestVacantRooms + VipVacantRooms;
            }
        }

        public int TotalAllocatedRooms
        {
            get
            {
                return RegularAllocatedRooms + GuestAllocatedRooms + VipAllocatedRooms;
            }
        }

        public int TotalRooms
        {
            get
            {
                return TotalVacantRooms + TotalAllocatedRooms;
            }
        }
    }
}
