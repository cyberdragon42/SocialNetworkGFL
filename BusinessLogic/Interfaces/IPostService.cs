﻿using BusinessLogic.Dto;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IPostService
    {
        public void CreatePost(Post post);

        IEnumerable<PostModel> GetUserPosts(string currentUserId);
        PostModel GetPost(string postId, string currentUserId);
        void LikePost(string postId, string returnUrl);
        void DislikePost(string postId, string returnUrl);
    }
}
