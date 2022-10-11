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
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostService postService;
        private readonly IUserService userService;
        private readonly IControllerHelper controllerHelper;

        public HomeController(ILogger<HomeController> logger, 
            IPostService postService,
            IUserService userService, 
            IControllerHelper controllerHelper)
        {
            _logger = logger;
            this.postService = postService;
            this.userService = userService;
            this.controllerHelper = controllerHelper;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            var id = controllerHelper.GetIdFromCurrentUser(HttpContext);
            var posts = postService.GetUserPosts(id);
            return View(posts);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Index([Bind("Content")] Post post)
        {
            if (ModelState.IsValid)
            {
                var id = controllerHelper.GetIdFromCurrentUser(HttpContext);
                post.UserId = id;
                post.Date = DateTime.UtcNow;

                postService.CreatePost(post);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Search(string keyword)
        {
            var users = userService.FindUsers(keyword);
            return View(users);
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
