using AutoMapper;
using BusinessLogic.Dto;
using BusinessLogic.Interfaces;
using Domain.Context;
using Domain.Models;
using Microsoft.AspNetCore.Http;
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

            for(var i=0; i < follows.Count; ++i)
            {
                follows[i].IsFollower = await IsFollower(id, follows[i].Id);
            }

            return follows;
        }

        public async Task<IEnumerable<ProfileModel>> GetUserFollowers(string userId)
        {
            var followers = await context.Subscriptions
                .Include(s => s.Follower)
                .Where(s => s.FollowingId == userId)
                .Select(s => mapper.Map<User, ProfileModel>(s.Follower))
                .ToListAsync();

            for (var i = 0; i < followers.Count; ++i)
            {
                followers[i].IsFollower = true;
                followers[i].IsFollowed = await IsFollowing(userId, followers[i].Id);
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

        /// <summary>
        ///is user with followerId a follower of current user
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="followerId"></param>
        /// <returns></returns>
        protected async Task<bool> IsFollower(string currentUserId, string followerId)
        {
            var subscription = await context.Subscriptions
                .Where(s=>s.FollowerId==followerId&&s.FollowingId==currentUserId)
                .FirstOrDefaultAsync();
            if (subscription != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// does current user follow user with followId
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="followId"></param>
        /// <returns></returns>
        protected async Task<bool> IsFollowing(string currentUserId, string followId)
        {
            var subscription = await context.Subscriptions
                .Where(s => s.FollowerId == currentUserId && s.FollowingId == followId)
                .FirstOrDefaultAsync();
            if (subscription != null)
            {
                return true;
            }
            return false;
        }
    }
}
