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

namespace SocialNetworkGFL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SocialNetworkContext _context;
        private readonly IPostService postService;

        public HomeController(ILogger<HomeController> logger, SocialNetworkContext context, IPostService service)
        {
            _logger = logger;
            _context = context;
            postService = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string content)
        {
            var post = new Post
            {
                Content = content,
                Date = DateTime.UtcNow,
                UserId = "A96F575B-31B9-4DA6-98CF-CBD0C115B809"
            };

            postService.CreatePost(post);
            return View();
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
