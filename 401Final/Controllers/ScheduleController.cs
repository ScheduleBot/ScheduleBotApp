﻿using Final401.Models;
using Final401.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final401.Controllers
{
    [Route("API/Schedule")]
    public class ScheduleController : Controller
    {
        private readonly ISchedule _context;

        public ScheduleController(ISchedule context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetSchedule")]
        [Route("Get/{id}")]
        public ActionResult<Schedule> Get(int id)
        {
            var schedule = _context.GetScheduleByID(id);
            if (schedule is null) return NotFound();
            else return schedule;
        }

        [HttpGet]
        [Route("GetAll/{userID}")]
        public ActionResult<List<Schedule>> GetAll(string userID)
        {
            try
            {
                return _context.GetUserSchedules(userID);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("New")]
        public IActionResult New([FromBody]Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                _context.CreateSchedule(schedule);
                return CreatedAtRoute("GetSchedule", new { id = schedule.ID }, schedule);
            }
            else return BadRequest();
        }

        [HttpPut]
        [Route("Update/{id}")]
        public IActionResult Update(int id, [FromBody]Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.UpdateSchedule(id, schedule);
                    return NoContent();
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
            }
            else return BadRequest();
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var schedule = _context.GetScheduleByID(id);
            if (schedule is null) return NotFound();
            else
            {
                _context.DeleteSchedule(schedule);
                return NoContent();
            }
        }

        [HttpDelete]
        [Route("DeleteAll/{userID}")]
        public IActionResult DeleteAll(string userID)
        {
            try
            {
                _context.DeleteAllUserSchedules(userID);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
