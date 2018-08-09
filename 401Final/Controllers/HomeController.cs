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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration Configuration;
        private ScheduleDBContext _context;
        
        /// <summary>
        /// default controller action
        /// </summary>
        /// <returns>home index view</returns>
        public HomeController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, ScheduleDBContext context)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            Configuration = configuration;
            _context = context;
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

            return View(await result.ToListAsync());
        }

        /// <summary>
        /// this doesn't currently do anything. Do we really need it?
        /// </summary>
        /// <returns>View ith ViewData</returns>
        public IActionResult Notes()
        {

            List<ScheduleItem> items = _context.ScheduleItems.Where(x => x.ScheduleID == 1).ToList();

            ViewData["Message"] = "This will display the Notes for the specified class.";

            return View(items);
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
