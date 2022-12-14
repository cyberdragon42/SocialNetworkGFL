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
using SocialNetworkGFL.Helpers;
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
        private readonly INotificationService notificationService;
        private readonly SocialNetworkContext context;

        public ProfileController(IUserService userService, IPostService postService,
            ICommentService commentService, INotificationService notificationService,
            IWebHostEnvironment appEnvironment,
            SocialNetworkContext context
            )
        {
            this.userService = userService;
            this.postService = postService;
            this.commentService = commentService;
            this.notificationService = notificationService;
            this.appEnvironment = appEnvironment;
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string id)
        {
            var currentUserId = HttpContext.GetIdFromCurrentUser();

            if (string.IsNullOrEmpty(id))
            {
                id = currentUserId;
            }

            var profile = await userService.GetProfileAsync(id,currentUserId);
            return View(profile);
        }

        [HttpGet]
        public async Task<IActionResult> UserFollows()
        {
            var currentUserId = HttpContext.GetIdFromCurrentUser();
            var follows = await userService.GetUserFollowsAsync(currentUserId);
            return View(follows);
        }

        [HttpGet]
        public async Task<IActionResult> UserFollowers()
        {
            var currentUserId = HttpContext.GetIdFromCurrentUser();
            var followers = await userService.GetUserFollowersAsync(currentUserId);
            return View(followers);
        }

        [HttpGet]
        public async Task<IActionResult> Follow(string id, string returnUrl)
        {
            var currentUserId = HttpContext.GetIdFromCurrentUser();
            await userService.FollowUserAsync(currentUserId, id);
            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        public async Task<IActionResult> Unfollow(string id, string returnUrl)
        {
            var currentUserId = HttpContext.GetIdFromCurrentUser();
            await userService.UnfollowUserAsync(currentUserId, id);
            return LocalRedirect(returnUrl);
        }


        [HttpGet]
        public async Task<IActionResult> Post(string id)
        {
            var currentUserId = HttpContext.GetIdFromCurrentUser();
            var post = await postService.GetPostAsync(id, currentUserId);
            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> Post(string id, CreateCommentDto commentDto)
        {
            if (ModelState.IsValid)
            {
                commentDto.Date = DateTime.UtcNow;
                commentDto.PostId = id;
                commentDto.UserId = HttpContext.GetIdFromCurrentUser();

                await commentService.AddCommentAsync(commentDto);
                return RedirectToAction("Post", new { id });
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Notifications()
        {
            var id = HttpContext.GetIdFromCurrentUser();
            var notifications = await notificationService.GetUserNotifications(id);
            return View(notifications);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var id = HttpContext.GetIdFromCurrentUser();
            var profile = await userService.GetProfileAsync(id, id);
            return View(profile);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([Bind("Name, UserName")] ProfileDto profile, IFormFile avatar)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.GetIdFromCurrentUser();
                var user = context.Users
                    .Where(x => x.Id == userId)
                    .FirstOrDefault();

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
