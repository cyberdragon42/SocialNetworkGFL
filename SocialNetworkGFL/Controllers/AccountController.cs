using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkGFL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetworkGFL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        public AccountController(UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Register(
            RegistrationModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = registerModel.UserName,
                    Email = registerModel.Email,
                    Name = registerModel.Name,
                };

               await userManager.CreateAsync(user, registerModel.Password);

                return RedirectToAction("Login");
            }

            return View(registerModel);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(loginModel.Username);
                if (user != null)
                {
                    await signInManager.PasswordSignInAsync(user, loginModel.Password, false, false);
                }

                return RedirectToAction("Index", "Profile");
            }

            return View(loginModel);
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
