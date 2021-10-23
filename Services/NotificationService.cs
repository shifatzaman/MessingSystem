using MessingSystem.Data;
using MessingSystem.Domain;
using MessingSystem.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Services
{
    public class NotificationService : INotificationService
    {
        private readonly AppDbContext dbContext;

        public NotificationService(AppDbContext context)
        {
            dbContext = context;
        }
        public void AddNotification(int creatorId, DateTime creationDate, string message, string url, int notificationFor = 0, bool sendToAllAdmins = false)
        {
            if (sendToAllAdmins)
            {
                var allAdmins = dbContext.Users.Where(u => u.Role == (int)UserRoles.Manager).Select(u=> u.UserId).ToList();

                var notifications = new List<Notification>();

                if (allAdmins != null && allAdmins.Count > 0)
                {
                    foreach(var adminUserId in allAdmins)
                    {
                        notifications.Add(new Notification
                        {
                            CreatedBy = creatorId,
                            CreatedOn = creationDate,
                            NotificationFor =adminUserId,
                            Message = message,
                            NotificationUrl = url,
                            Seen = false
                        });
                    }

                    SaveNotifications(notifications);
                }
            }
            else if (notificationFor > 0)
            {
                var notification = new Notification
                {
                    CreatedBy = creatorId,
                    CreatedOn = creationDate,
                    NotificationFor = notificationFor,
                    Message = message,
                    NotificationUrl = url,
                    Seen = false
                };

                SaveNotification(notification);
            }
            else
            {
                return;
            }
        }

        public (List<Notification>, int) GetNotifications(int userId)
        {
            var notifications = dbContext.Notifications.Where(it => it.NotificationFor == userId).OrderByDescending(it => it.CreatedOn).ToList();

            if (notifications != null && notifications.Count > 0)
            {
                return (notifications, notifications.Where(it => !it.Seen).Count());
            }

            return (null, 0);
        }

        public void UpdateSeenStatus(int notificationId)
        {
            var notification = dbContext.Notifications.Where(it => it.Id == notificationId).FirstOrDefault();

            if (notification != null)
            {
                notification.Seen = true;
                dbContext.Notifications.Update(notification);
                dbContext.SaveChanges();
            }
        }

        private void SaveNotification(Notification notification)
        {
            dbContext.Notifications.Add(notification);
            dbContext.SaveChanges();
        }

        private void SaveNotifications(IList<Notification> notifications)
        {
            dbContext.Notifications.AddRange(notifications);
            dbContext.SaveChanges();
        }
    }
}
