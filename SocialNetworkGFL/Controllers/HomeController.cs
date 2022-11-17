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
using SocialNetworkGFL.Helpers;
using BusinessLogic.Dto;

namespace SocialNetworkGFL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostService postService;
        private readonly IUserService userService;

        public HomeController(ILogger<HomeController> logger, 
            IPostService postService,
            IUserService userService)
        {
            _logger = logger;
            this.postService = postService;
            this.userService = userService;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            var id = HttpContext.GetIdFromCurrentUser();
            var posts = postService.GetUserFeed(id);
            return View(posts);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Index(CreatePostDto postDto)
        {
            if (ModelState.IsValid)
            {
                var id = HttpContext.GetIdFromCurrentUser();
                postDto.UserId = id;
                postDto.Date = DateTime.UtcNow;
                await postService.CreatePostAsync(postDto);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Search(string keyword)
        {
            var id = HttpContext.GetIdFromCurrentUser();
            var users = await userService.FindUsersAsync(keyword, id);
            var model = new SearchDto
            {
                Keyword=keyword,
                Users=users
            };
            return View(model);
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
