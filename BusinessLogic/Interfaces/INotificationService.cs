using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface INotificationService
    {
        public Task CreateNotification(Notification notification);
        public Task<IEnumerable<Notification>> GetUserNotifications(string userId);
        public Task<int> GetUnreadNotificationsCount(string userId);

    }
}
