using Final401.Data;
using Final401.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final401.Controllers
{
    /// <summary>
    /// /Api controller default route URL
    /// </summary>
    [Route("API/ChatLog")]
    public class ChatLogController : Controller
    {
        private ScheduleDBContext _context;

        //sets DbContext to use ScheduleDbContext
        public ChatLogController(ScheduleDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// post method to API chat log table using route API/ChatLog/New
        /// </summary>
        /// <param name="chatLog">JSON string body from bot</param>
        /// <returns>status code if entry added to databse, or bad request if failed</returns>
        [HttpPost("New")]
        public IActionResult Create([FromBody]ChatLog chatLog)
        {

            //var result = JObject.Parse(chatLog);
            string savedString = chatLog.Chat;

            if (ModelState.IsValid)
            {
                ChatLog newLog = new ChatLog();

                newLog.ID = chatLog.ID;
                newLog.TimeStamp = DateTime.Now;
                newLog.Chat = savedString;

                _context.ChatLogs.Add(newLog);
                _context.SaveChanges();

                return Ok();
            }
            else return BadRequest();

        }
       
    }
}
