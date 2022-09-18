using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetworkGFL.Controllers
{
    public class ProfileController : Controller
    {
        private IUserService userService;
        private IPostService postService;
        private ICommentService commentService;
        private string tempUserId = "A96F575B-31B9-4DA6-98CF-CBD0C115B809";

        public ProfileController(IUserService userService, IPostService postService,
            ICommentService commentService)
        {
            this.userService = userService;
            this.postService = postService;
            this.commentService = commentService;
        }

        public ActionResult Index()
        {
            var user = userService.GetUser(tempUserId);
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
                comment.UserId = tempUserId;

                commentService.AddComment(comment);
                var post = postService.GetPost(id);
                return View(post);
            }

            return RedirectToAction("Index");
        }

    }
}
