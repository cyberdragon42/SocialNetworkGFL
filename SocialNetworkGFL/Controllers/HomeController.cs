using Domain.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SocialNetworkGFL.Models;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Services;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace SocialNetworkGFL.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostService postService;
        public IControllerHelper controllerHelper;
        private string tempUserId = "669e16bf-ee7e-4523-930c-1f7566278e9d";

        public HomeController(ILogger<HomeController> logger, IPostService service,IControllerHelper controllerHelper)
        {
            _logger = logger;
            postService = service;
            this.controllerHelper = controllerHelper;
        }

        public IActionResult Index()
        {
            var id = controllerHelper.GetIdFromCurrentUser(HttpContext);
            var posts = postService.GetUserFeed(id);
            return View(posts);
        }

        [HttpPost]
        public IActionResult Index(string content, [Bind("Date,Content")] Post post)
        {
            if (ModelState.IsValid)
            {
                var id = controllerHelper.GetIdFromCurrentUser(HttpContext);
                post.UserId = id;
                post.Date = DateTime.UtcNow;

                var posts = postService.GetUserFeed(id);
                postService.CreatePost(post);
                return View(posts);
            }

            return RedirectToAction("Index");
        }

        public ActionResult ToggleLike(string postId)
        {
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
