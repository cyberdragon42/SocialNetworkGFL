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
        private IUserService userService;
        private IPostService postService;
        private ICommentService commentService;
        public IControllerHelper controllerHelper;
        private string tempUserId = "669e16bf-ee7e-4523-930c-1f7566278e9d";

        public ProfileController(IUserService userService, IPostService postService,
            ICommentService commentService, IControllerHelper controllerHelper)
        {
            this.userService = userService;
            this.postService = postService;
            this.commentService = commentService;
            this.controllerHelper = controllerHelper;
        }

        public ActionResult Index(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = controllerHelper.GetIdFromCurrentUser(HttpContext);
            }
            var user = userService.GetUser(id);
            return View(user);
        }

        public ActionResult Followers()
        {

            return View();
        }

        public ActionResult Follows()
        {

            return View();
        }

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
                var post = postService.GetPost(id);
                return View(post);
            }

            return RedirectToAction("Index");
        }

    }
}
