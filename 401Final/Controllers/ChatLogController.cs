using Final401.Data;
using Final401.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final401.Controllers
{
    [Route("API/ChatLog")]
    public class ChatLogController : Controller
    {
        private ScheduleDBContext _context;

        public ChatLogController(ScheduleDBContext context)
        {
            _context = context;
        }

        [HttpPost("New")]
        public IActionResult Create(string chatLog)
        {

            if (ModelState.IsValid)
            {
                ChatLog newLog = new ChatLog();
                newLog.TimeStamp = DateTime.Now;
                newLog.Chat = chatLog;

                _context.ChatLogs.Add(newLog);
                _context.SaveChanges();

                return Ok();
            }
            else return BadRequest();

        }
       
    }
}
