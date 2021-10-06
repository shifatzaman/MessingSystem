using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Models
{
    public class MessMemberViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Bnumb { get; set; }

        public string Rank { get; set; }

        public string Appt { get; set; }

        public string Unit { get; set; }
        public string ContactNo { get; set; }
        public string RoomNo { get; set; }

        public DateTime DateOfEntry { get; set; }

        public string MaritialStatus { get; set; }
        public int MemberStatus { get; set; }

        public string MemberStatusString { 
            get {

                return MemberStatus == (int)MessingSystem.Enums.MemberStatus.Dining ? "Dining" : "Non-Dining";
            } 
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public int UserId { get; set; }
    }
}
