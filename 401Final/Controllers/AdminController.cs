using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Final401.Data;
using Final401.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final401.Controllers
{
    public class AdminController : Controller
    {
        //db context for class
        private ScheduleDBContext _context;

        /// <summary>
        /// controller constructor
        /// </summary>
        /// <param name="context">selected DbContext</param>
        public AdminController(ScheduleDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Default index view
        /// </summary>
        /// <returns>view</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// method to view recent activity
        /// </summary>
        /// <returns>view with results as list</returns>

        [HttpGet]
        public async Task<IActionResult> RecentActivity()
        {
            var result = from x in _context.ChatLogs
                               select x;

            var resultList = await result.ToListAsync();

            if(result == null)
            {
                return RedirectToAction("RecentActivity", "Home");
            }

            return View(await result.ToListAsync());
           
        }
    }
}