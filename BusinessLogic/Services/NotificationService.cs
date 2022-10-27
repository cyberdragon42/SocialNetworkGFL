using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using Domain.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class NotificationService: INotificationService
    {
        private readonly SocialNetworkContext context;
        public NotificationService(SocialNetworkContext context)
        {
            this.context = context;
        }

        public async Task CreateNotification(Notification notification)
        {
            context.Notifications.Add(notification);
            await context.SaveChangesAsync();
        }

        public async Task<int> GetUnreadNotificationsCount(string userId)
        {
            return await context.Notifications
                .Where(n => n.RecieverId == userId && n.IsReceived == false)
                .CountAsync();
        }

        public async Task<IEnumerable<Notification>> GetUserNotifications(string userId)
        {
            var notifications = await context.Notifications
                .Where(n => n.RecieverId == userId)
                .OrderByDescending(n=>n.Date)
                .Take(30)
                .ToListAsync();

            //var tempArray = new Notification[updatedNotifications.Count];
            //updatedNotifications.CopyTo(tempArray);

            //var userNotifications = tempArray.ToList();

            foreach (var n in notifications)
            {
                n.IsReceived = true;
            }

            await context.SaveChangesAsync();
            return notifications;
        }
    }
}
