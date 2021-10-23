using MessingSystem.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Services
{
    public interface INotificationService
    {
       void AddNotification(int creatorId, DateTime creationData, string message, string url,  int notificationFor = 0, bool sendToAllAdmins = false);

        (List<Notification>, int) GetNotifications(int userId);

        void UpdateSeenStatus(int notificationId);
    };
}
