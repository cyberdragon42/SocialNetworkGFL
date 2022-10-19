using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Domain.Models;
using Domain.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Helpers;
using System.Web;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using BusinessLogic.Dto;

namespace SocialNetworkGFL.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IUserService userService;
        private readonly IPostService postService;
        private readonly ICommentService commentService;
        private readonly IWebHostEnvironment appEnvironment;
        private readonly SocialNetworkContext context;

        public ProfileController(IUserService userService, IPostService postService,
            ICommentService commentService, IWebHostEnvironment appEnvironment,
            SocialNetworkContext context
            )
        {
            this.userService = userService;
            this.postService = postService;
            this.commentService = commentService;
            this.appEnvironment = appEnvironment;
            this.context = context;
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

        [HttpGet]
        public ActionResult Edit()
        {
            var id = HttpContext.GetIdFromCurrentUser();
            var profile = userService.GetProfile(id, id);
            return View(profile);
        }

        [HttpPost]
        public async Task<ActionResult> Edit([Bind("Name, UserName")] ProfileModel profile, IFormFile avatar)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.GetIdFromCurrentUser();
                var user = context.Users
                    .Where(x => x.Id == userId)
                    .FirstOrDefault();

                //var file = data["avatar"];
                if (avatar != null)
                {
                    var guidFileName = Guid.NewGuid();
                    var extension = Path.GetExtension(avatar.FileName);
                    var path = $"/Avatars/{guidFileName}{extension}";
                    using (var fileStream = new FileStream(appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await avatar.CopyToAsync(fileStream);
                    }

                    var oldPath = $"{appEnvironment.WebRootPath}/Avatars/{user.AvatarId}{user.AvatarExtension}";
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }

                    user.AvatarId = guidFileName;
                    user.AvatarExtension = extension;
                }

                user.Name = profile.Name;
                user.UserName = profile.UserName;
                context.SaveChanges();

                return RedirectToAction("Edit");
            }

            return View(profile);
        }

    }
}
