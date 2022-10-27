using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SocialNetworkGFL.Helpers;
using BusinessLogic.Services;
using BusinessLogic.Interfaces;
using Domain.Models;

namespace SocialNetworkGFL.Hubs
{
    public class NotificationsHub: Hub
    {
        private readonly IUserService userService;
        private readonly INotificationService notificationService;
        public NotificationsHub(IUserService userService, INotificationService notificationService):base()
        {
            this.userService = userService;
            this.notificationService = notificationService;
        }
        public async Task Send(string mesage)
        {
            await this.Clients.All.SendAsync("Send", mesage);
        }

        public async Task SendNotification(string message, string userId)
        {
            var currentUserId = Context.GetHttpContext().GetIdFromCurrentUser();
            var currentUser = userService.GetUser(currentUserId);
            var receiver = userService.GetUser(userId);

            var notificationForReceiver = new Notification()
            {
                Date = DateTime.UtcNow,
                Reciever = receiver
            };

            var notificationForSender = new Notification()
            {
                Date = DateTime.UtcNow,
                Reciever = currentUser
            };

            switch (message)
            {
                case "follow":
                    notificationForSender.Content = $"You followed {receiver.UserName}";
                    notificationForReceiver.Content = $"{currentUser.UserName} followed you";
                    break;

                case "unfollow":
                    notificationForSender.Content = $"You unfollowed {receiver.UserName}";
                    notificationForReceiver.Content = $"{currentUser.UserName} unfollowed you";
                    break;

                default:
                    break;
            }

            await notificationService.CreateNotification(notificationForReceiver);
            await notificationService.CreateNotification(notificationForSender);
            var senderNotificationCount = await notificationService.GetUnreadNotificationsCount(currentUserId);
            var receiverNotificationCount = await notificationService.GetUnreadNotificationsCount(userId);

            await Clients.Caller.SendAsync("SendNotification", senderNotificationCount);
            await Clients.Others.SendAsync("SendNotification", receiverNotificationCount);
        }
    }
}
