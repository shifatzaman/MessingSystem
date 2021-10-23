using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Domain
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Message { get; set; }

        public int NotificationFor { get; set; }

        public string NotificationUrl { get; set; }

        public bool Seen { get; set; }
    }
}
