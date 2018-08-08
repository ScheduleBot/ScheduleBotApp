using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Final401.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Final401.Data;
using Microsoft.EntityFrameworkCore;

namespace Final401.Controllers
{

    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {
        //injecting schedule DbContext
        private ScheduleDBContext _context;

        /// <summary>
        /// homecontroller contstructor setting ScheduleDbContext
        /// </summary>
        /// <param name="context">ScheduleDbContext</param>
        public HomeController(ScheduleDBContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// default controller action
        /// </summary>
        /// <returns>home index view</returns>
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration Configuration;

        public HomeController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// method to retrieve list of bot recent activity
        /// </summary>
        /// <returns>first 20 entries from ChatLog table in Schedule database</returns>
        public async Task<IActionResult> RecentActivity()
        {
            ViewData["Message"] = "Bot 202's recent activity.";

            var result = from x in _context.ChatLogs.Take(20)
                         orderby x.TimeStamp descending
                         select x;

            if (result == null)
            {
                return RedirectToAction("RecentActivity", "Home");
            }

            return View(await result.ToListAsync());
        }

        /// <summary>
        /// this doesn't currently do anything. Do we really need it?
        /// </summary>
        /// <returns>View ith ViewData</returns>
        public IActionResult ItemList()
        {
            ViewData["Message"] = "This will display the items the bot can read.";

            return View();
        }

        /// <summary>
        /// method to take user to TestBot view
        /// </summary>
        /// <returns>view</returns>
        public IActionResult TestBot()
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
