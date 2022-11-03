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
        public PostController(IPostService postService)
        {
            this.postService = postService;
        }

        [HttpGet]
        public IActionResult LikePost(string postId, string returnUrl)
        {
            var currentUserId = HttpContext.GetIdFromCurrentUser();
            postService.LikePost(postId, currentUserId);
            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        public IActionResult DislikePost(string postId, string returnUrl)
        {
            var currentUserId = HttpContext.GetIdFromCurrentUser();
            postService.DislikePost(postId, currentUserId);
            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        public IActionResult LikedPosts()
        {
            var currentUserId = HttpContext.GetIdFromCurrentUser();
            var posts = postService.LikedPosts(currentUserId);
            return View(posts);
        }

        [HttpGet]
        public IActionResult UserComments()
        {
            var currentUserId = HttpContext.GetIdFromCurrentUser();
            var comments = postService.UserComments(currentUserId);
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
