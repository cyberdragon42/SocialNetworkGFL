using AutoMapper;
using BusinessLogic.Dto;
using BusinessLogic.Interfaces;
using Domain.Context;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class UserService: IUserService
    {
        private readonly SocialNetworkContext context;
        private readonly IMapper mapper;
        public UserService(SocialNetworkContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task FollowUser(string userId, string id)
        {
            var subscription = new Subscription
            {
                FollowerId = userId,
                FollowingId = id
            };

            context.Subscriptions.Add(subscription);
            await context.SaveChangesAsync();
        }

        public async Task UnfollowUser(string userId, string id)
        {
            var subscription = context.Subscriptions
                .Where(s => s.FollowerId == userId && s.FollowingId == id)
                .FirstOrDefault();
            context.Subscriptions.Remove(subscription);
            await context.SaveChangesAsync();
        }

        public ProfileModel GetProfile(string userId, string currentUserId)
        {
            var user = context.Users.
                Where(u => u.Id == userId)
                .Include(u => u.Posts.OrderByDescending(p => p.Date))
                .ThenInclude(p => p.Comments)
                .Include(u=>u.Followers)
                .Include(u=>u.Followings)
                .FirstOrDefault();

            var profile = mapper.Map<User, ProfileModel>(user);

            if (user.Followers.FirstOrDefault(f => f.FollowerId == currentUserId) != null)
            {
                profile.IsFollowed = true;
            }

            if (user.Followings.FirstOrDefault(f => f.FollowingId == currentUserId) != null)
            {
                profile.IsFollower = true;
            }

            return profile;
        }

        public User GetUser(string userId)
        {
            var user = context.Users.
                Where(u => u.Id == userId)
                .Include(u=>u.Posts.OrderByDescending(p => p.Date))
                .ThenInclude(p=>p.Comments)
                .FirstOrDefault();

            return user;
        }

        public async Task<IEnumerable<ProfileModel>> GetUserFollows(string id)
        {
            var follows = await context.Subscriptions
                .Include(s => s.Following)
                .Where(s => s.FollowerId == id)
                .Select(s=> mapper.Map <User, ProfileModel>(s.Following))
                .ToListAsync();

            return follows;
        }

        public async Task<IEnumerable<ProfileModel>> GetUserFollowers(string userId)
        {
            var subscriptions = context.Subscriptions
                .Include(s => s.Follower)
                .Where(s => s.FollowingId == userId);

            var followers = new List<ProfileModel>();
            foreach (var s in subscriptions)
            {
                var profile = new ProfileModel
                {
                    Id = s.Follower.Id,
                    Name = s.Follower.Name,
                    Email = s.Follower.Email,
                    UserName = s.Follower.UserName,
                    Posts = s.Follower.Posts,
                };

                //if user reads currentuser
                if (s.Follower.Followings.FirstOrDefault(f => f.FollowingId == userId) != null)
                {
                    profile.IsFollower = true;
                }

                followers.Add(profile);
            }

            return followers;
        }

        public IEnumerable<ProfileModel> FindUsers(string keyword)
        {
            var users = context.Users.Where(
                u => u.Name.Contains(keyword) || u.UserName.Contains(keyword))
                .Select(user=> mapper.Map<User, ProfileModel>(user))
                .ToList();

            return users;
        }
    }
}
