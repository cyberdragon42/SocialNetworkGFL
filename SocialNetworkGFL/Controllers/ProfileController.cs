using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Helpers;

namespace SocialNetworkGFL.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IUserService userService;
        private readonly IPostService postService;
        private readonly ICommentService commentService;

        public ProfileController(IUserService userService, IPostService postService,
            ICommentService commentService)
        {
            this.userService = userService;
            this.postService = postService;
            this.commentService = commentService;
        }

        [HttpGet]
        public ActionResult Index(string id)
        {
            var currentUserId = HttpContext.GetIdFromCurrentUser();

            if (string.IsNullOrEmpty(id))
            {
                id = currentUserId;
            }

            var profile = userService.GetProfile(id,currentUserId);
            return View(profile);
        }

        [HttpGet]
        public async Task<ActionResult> UserFollows()
        {
            var userId = HttpContext.GetIdFromCurrentUser();
            var follows = await userService.GetUserFollows(userId);
            return View(follows);
        }

        [HttpGet]
        public async Task<ActionResult> UserFollowers()
        {
            var userId = HttpContext.GetIdFromCurrentUser();
            var followers = await userService.GetUserFollowers(userId);
            return View(followers);
        }

        [HttpGet]
        public async Task<ActionResult> Follow(string id, string returnUrl)
        {
            var userId = HttpContext.GetIdFromCurrentUser();
            await userService.FollowUser(userId, id);
            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        public async Task<ActionResult> Unfollow(string id, string returnUrl)
        {
            var userId = HttpContext.GetIdFromCurrentUser();
            await userService.UnfollowUser(userId, id);
            return LocalRedirect(returnUrl);
        }


        [HttpGet]
        public ActionResult Post(string id)
        {
            var post = postService.GetPost(id);
            return View(post);
        }

        [HttpPost]
        public ActionResult Post(string id, [Bind("Date,Content,UserId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.Date = DateTime.UtcNow;
                comment.PostId = id;
                comment.UserId = HttpContext.GetIdFromCurrentUser();

                commentService.AddComment(comment);
                return RedirectToAction("Post", new { id });
            }

            return RedirectToAction("Index");
        }

    }
}
