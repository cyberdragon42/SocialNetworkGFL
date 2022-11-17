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

        public async Task FollowUserAsync(string currentUserId, string userId)
        {
            var subscription = new Subscription
            {
                FollowerId = currentUserId,
                FollowingId = userId
            };

            context.Subscriptions.Add(subscription);
            await context.SaveChangesAsync();
        }

        public async Task UnfollowUserAsync(string currentUserId, string userId)
        {
            var subscription = context.Subscriptions
                .Where(s => s.FollowerId == currentUserId && s.FollowingId == userId)
                .FirstOrDefault();
            context.Subscriptions.Remove(subscription);
            await context.SaveChangesAsync();
        }

        public async Task<ProfileDto> GetProfileAsync(string userId, string currentUserId)
        {
            var user = context.Users.
                Where(u => u.Id == userId)
                .Include(u => u.Posts.OrderByDescending(p => p.Date)).ThenInclude(p => p.Comments)
                .Include(u => u.Posts.OrderByDescending(p => p.Date)).ThenInclude(p=>p.Likes)
                .Include(u=>u.Followers)
                .Include(u=>u.Followings)
                .FirstOrDefault();

            var profile = mapper.Map<User, ProfileDto>(user);

            var postModels = new List<ExtendedPostDto>();
            foreach (var p in user.Posts)
            {
                var postModel = mapper.Map<Post, ExtendedPostDto>(p);
                postModel.IsLiked = p.Likes.Any(l => l.UserId == currentUserId);
                postModels.Add(postModel);
            }

            profile.Posts = postModels;

            if (profile.Id == currentUserId)
            {
                profile.IsOwnProfile = true;
            }

            if(await IsFollowerAsync(currentUserId, userId))
            {
                profile.IsFollower = true;
            }

            if(await IsFollowingAsync(currentUserId, userId))
            {
                profile.IsFollowed = true;
            }

            return profile;
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            var user = await context.Users.
                Where(u => u.Id == userId)
                .Include(u=>u.Posts.OrderByDescending(p => p.Date))
                .ThenInclude(p=>p.Comments)
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<IEnumerable<ProfileDto>> GetUserFollowsAsync(string currentUserId)
        {
            var follows = await context.Subscriptions
                .Include(s => s.Following)
                .Where(s => s.FollowerId == currentUserId)
                .Select(s=> mapper.Map <User, ProfileDto>(s.Following))
                .ToListAsync();

            for(var i=0; i < follows.Count; ++i)
            {
                follows[i].IsFollower = await IsFollowerAsync(currentUserId, follows[i].Id);
                follows[i].IsFollowed = true;
            }

            return follows;
        }

        public async Task<IEnumerable<ProfileDto>> GetUserFollowersAsync(string currentUserId)
        {
            var followers = await context.Subscriptions
                .Include(s => s.Follower)
                .Where(s => s.FollowingId == currentUserId)
                .Select(s => mapper.Map<User, ProfileDto>(s.Follower))
                .ToListAsync();

            for (var i = 0; i < followers.Count; ++i)
            {
                followers[i].IsFollower = true;
                followers[i].IsFollowed = await IsFollowingAsync(currentUserId, followers[i].Id);
            }

            return followers;
        }

        public async Task<IEnumerable<ProfileDto>> FindUsersAsync(string keyword, string currentUserId)
        {
            var users = await context.Users.Where(
                u => u.Name.Contains(keyword) || u.UserName.Contains(keyword))
                .Select(user=> mapper.Map<User, ProfileDto>(user))
                .ToListAsync();

            for (var i = 0; i < users.Count; ++i)
            {
                users[i].IsFollower = await IsFollowerAsync(currentUserId, users[i].Id);
                users[i].IsFollowed = await IsFollowingAsync(currentUserId, users[i].Id);
                users[i].IsOwnProfile = users[i].Id == currentUserId;
            }

            return users;
        }

        /// <summary>
        ///is user with followerId a follower of current user
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="followerId"></param>
        /// <returns></returns>
        protected async Task<bool> IsFollowerAsync(string currentUserId, string followerId)
        {
            return await context.Subscriptions
                .AnyAsync(s => s.FollowerId == followerId && s.FollowingId == currentUserId);

        }

        /// <summary>
        /// does current user follow user with followId
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="followId"></param>
        /// <returns></returns>
        protected async Task<bool> IsFollowingAsync(string currentUserId, string followId)
        {
           return await context.Subscriptions
                .AnyAsync(s => s.FollowerId == currentUserId && s.FollowingId == followId);
        }
    }
}
