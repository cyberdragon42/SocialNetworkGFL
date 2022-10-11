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

namespace SocialNetworkGFL.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IUserService userService;
        private readonly IPostService postService;
        private readonly ICommentService commentService;
        private readonly IControllerHelper controllerHelper;

        public ProfileController(IUserService userService, IPostService postService,
            ICommentService commentService, IControllerHelper controllerHelper)
        {
            this.userService = userService;
            this.postService = postService;
            this.commentService = commentService;
            this.controllerHelper = controllerHelper;
        }

        [HttpGet]
        public ActionResult Index(string id)
        {
            var currentUserId = controllerHelper.GetIdFromCurrentUser(HttpContext);
            if (string.IsNullOrEmpty(id))
            {
                id = controllerHelper.GetIdFromCurrentUser(HttpContext);
            }

            var profile = userService.GetProfile(id,currentUserId);
            return View(profile);
        }

        [HttpGet]
        public async Task<ActionResult> UserFollows()
        {
            var userId = controllerHelper.GetIdFromCurrentUser(HttpContext);
            var follows = await userService.GetUserFollows(userId);
            return View(follows);
        }

        [HttpGet]
        public async Task<ActionResult> UserFollowers()
        {
            var userId = controllerHelper.GetIdFromCurrentUser(HttpContext);
            var followers = await userService.GetUserFollowers(userId);
            return View(followers);
        }

        [HttpGet]
        public async Task<ActionResult> Follow(string id, string returnUrl)
        {
            var userId = controllerHelper.GetIdFromCurrentUser(HttpContext);
            await userService.FollowUser(userId, id);
            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        public async Task<ActionResult> Unfollow(string id, string returnUrl)
        {
            var userId = controllerHelper.GetIdFromCurrentUser(HttpContext);
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
                comment.UserId = controllerHelper.GetIdFromCurrentUser(HttpContext);

                commentService.AddComment(comment);
                return RedirectToAction("Post", new { id });
            }

            return RedirectToAction("Index");
        }

    }
}
