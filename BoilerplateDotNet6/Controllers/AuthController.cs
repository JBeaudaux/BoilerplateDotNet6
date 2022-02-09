using AspNetCore.Identity.Mongo.Model;
using BoilerplateDotNet6.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BoilerplateDotNet6.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<MongoUser> _userManager;
        private readonly RoleManager<MongoRole> _roleManager;
        private readonly SignInManager<MongoUser> _signInManager;
        //private readonly EponaService _eponaService;
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager">(User, DemoUser, PowerUser, Admin)</param>
        /// <param name="signInManager"></param>
        /// <param name="bookService"></param>
        /// <param name="logger"></param>
        public AuthController(UserManager<MongoUser> userManager,
                              RoleManager<MongoRole> roleManager,
                              SignInManager<MongoUser> signInManager,
                              ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            // If a user is already authenticated, go straight to next step
            /*if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "ControllerName");
            }*/

            return View(new LoginViewModel());
        }

        // Used to capture the login
        // ToDo : allow loging with email or username
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (String.IsNullOrEmpty(vm.Email) || String.IsNullOrEmpty(vm.Password))
            {
                // Do not reveal that the user does not exist or is not confirmed
                // ToDo : Add validation showing login failed
                return RedirectToAction("Login", "Auth");
            }

            var user = await _userManager.FindByEmailAsync(vm.Email);

            // User does not exist or has not been verified yet
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Do not reveal that the user does not exist or is not confirmed
                // ToDo : Add validation showing login failed
                return RedirectToAction("Login", "Auth");
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, vm.Password, vm.RememberMe, true);
            if (result.Succeeded)
            {
                var admrole = await _roleManager.FindByNameAsync("Admin");
                var prorole = await _roleManager.FindByNameAsync("Owner");
                var usrrole = await _roleManager.FindByNameAsync("User");
                var suprole = await _roleManager.FindByNameAsync("SuperUser");
                if (user.Roles.Contains(admrole.Id.ToString()) || user.Roles.Contains(prorole.Id.ToString()))
                {
                    return RedirectToAction("Index", "AdminSideController");
                }
                else if (user.Roles.Contains(usrrole.Id.ToString()) || user.Roles.Contains(suprole.Id.ToString()))
                {
                    return RedirectToAction("IndexDash", "UserSideController");
                }
            }
            else
            {
                // Do not reveal that the user does not exist or is not confirmed
                // ToDo : Add validation showing login failed
                return RedirectToAction("Login", "Auth");
            }

            // Do not reveal that the user does not exist or is not confirmed
            // ToDo : Add validation showing login failed
            return RedirectToAction("Login", "Auth");
        }

        /*[HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }*/


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return View("Login", new LoginViewModel());
        }
    }
}