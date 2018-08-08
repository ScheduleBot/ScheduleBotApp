using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Final401.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Final401.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration Configuration;


        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            Configuration = configuration;
        }



        /// <summary>
        /// Show the Register View
        /// </summary>
        /// <returns>The Register view </returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// adds new user to user database and creates claims with the respective information
        /// </summary>
        /// <param name="rvm"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel rvm)
        {
            //if (the forms are filled correctly)
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = rvm.Email,
                    Email = rvm.Email,
                };
                //inserts user data into userdb
                var result = await _userManager.CreateAsync(user, rvm.Password);

                //after user data successfully placed in db
                if (result.Succeeded)
                {
                    // create new claim
                    Claim emailClaim = new Claim(ClaimTypes.Email, user.Email, ClaimValueTypes.Email);


                   await _userManager.AddClaimAsync(user, emailClaim);

                   await _userManager.AddToRoleAsync(user, ApplicationRoles.Member);


                    if (user.Email.Contains("@"))
                    {

                        await _userManager.AddToRoleAsync(user, ApplicationRoles.Admin);
                       
                        return RedirectToAction("Login", "Account");
                    }
                    
                }

            }
            return View(rvm);
        }

        /// <summary>
        /// Shows the login page
        /// </summary>
        /// <returns>the login view</returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }

        /// <summary>
        /// Takes the information from the login view forum and checks the DB. If it exists logs in the user
        /// </summary>
        /// <param name="lvm">LoginViewModel</param>
        /// <returns>The same login view or home index or admin index</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel lvm)
        {
            var result = await _signInManager.PasswordSignInAsync(lvm.Email, lvm.Password, false, false);
            var user = await _userManager.FindByEmailAsync(lvm.Email);
            if (ModelState.IsValid)
            {
                if (result.Succeeded)
                {
                    return RedirectToAction("TestBot", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Either your email or password was incorrect");
                }
            }

            return View(lvm);
        }

        /// <summary>
        /// Logs the user out
        /// </summary>
        /// <returns>Home index</returns>
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["LoggedOut"] = "User Logged Out";

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Starts the external login/resgiter process
        /// </summary>
        /// <param name="provider"> a string of the provider</param>
        /// <returns> a challenge </returns>
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string remoteError = null)
        {
            if (remoteError != null)
            {
                TempData["ErrorMessage"] = "Error from Provider";
                return RedirectToAction(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (result.Succeeded)
            {
               return RedirectToAction("ItemList", "Home");
            }

            return View("ExternalLogin", new ExternalLoginViewModel { });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel elvm)
        {
            if (ModelState.IsValid)
            {
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    TempData["Error"] = "Error loading information";
                }

                var user = new ApplicationUser
                {
                    Email = elvm.Email
                };

                Claim emailClaim = new Claim(ClaimTypes.Email, user.Email, ClaimValueTypes.Email);

                if (user.Email == "anthonydgreen90@gmail.com")
                {
                    await _userManager.AddToRoleAsync(user, ApplicationRoles.Admin);

                    return RedirectToAction("ItemList", "Home");
                }

                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {

                    result = await _userManager.AddLoginAsync(user, info);

                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);

                        return RedirectToAction("ItemList", "Home");
                    }
                }
            }
            return RedirectToAction("ExternalLoginCallback", "Account", elvm);
        }
    }
}
