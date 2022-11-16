using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkGFL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetworkGFL.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService postService;
        private readonly ICommentService commentService;
        public PostController(IPostService postService, ICommentService commentService)
        {
            this.postService = postService;
            this.commentService = commentService;
        }

        [HttpGet]
        public async Task<IActionResult> LikePost(string postId, string returnUrl)
        {
            var currentUserId = HttpContext.GetIdFromCurrentUser();
            await postService.LikePostAsync(postId, currentUserId);
            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        public async Task<IActionResult> DislikePost(string postId, string returnUrl)
        {
            var currentUserId = HttpContext.GetIdFromCurrentUser();
            await postService.DislikePostAsync(postId, currentUserId);
            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        public IActionResult LikedPosts()
        {
            var currentUserId = HttpContext.GetIdFromCurrentUser();
            var posts = postService.GetLikedPosts(currentUserId);
            return View(posts);
        }

        [HttpGet]
        public async Task<IActionResult> UserComments()
        {
            var currentUserId = HttpContext.GetIdFromCurrentUser();
            var comments = await commentService.GetUserCommentsAsync(currentUserId);
            return View(comments);
        }

        //[HttpGet]
        //public IActionResult Delete(string id, string returnUrl)
        //{
        //    postService.Delete(id);
        //    return LocalRedirect(returnUrl);
        //}
    }
}
