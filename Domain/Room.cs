using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Domain
{
    public class Room
    {
        [Key]
        public int Id { get; set; }

        public int Type { get; set; }

        public string RoomNo { get; set; }

        public string AllocatedTo { get; set; }

        public bool IsAllocated { get; set; }

        public DateTime? DateOfEntry { get; set; }
    }
}
