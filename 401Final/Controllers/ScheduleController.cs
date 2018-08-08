using Final401.Models;
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

        //---------------- READ Methods ----------------//

        /// <summary>
        /// Gets a schedule by its ID.
        /// </summary>
        /// <param name="id">The ID of the schedule.</param>
        /// <returns>A schedule.</returns>
        [HttpGet("Get/{id}", Name = "GetSchedule")]
        public ActionResult<Schedule> Get(int id)
        {
            var schedule = _context.GetScheduleByID(id);
            if (schedule is null) return NotFound();
            else return schedule;
        }

        /// <summary>
        /// Gets all of a user's schedules.
        /// </summary>
        /// <param name="userID">The user's ID.</param>
        /// <returns>A list of schedules.</returns>
        [HttpGet("GetAll/{userID}")]
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

        //---------------- CREATE Methods ----------------//

        /// <summary>
        /// Adds a new schedule to the database.
        /// </summary>
        /// <param name="schedule">The schedule being added.</param>
        /// <returns>An action result.</returns>
        [HttpPost("New")]
        public IActionResult New([FromBody]Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                _context.CreateSchedule(schedule);
                return CreatedAtRoute("GetSchedule", new { id = schedule.ID }, schedule);
            }
            else return BadRequest();
        }

        //---------------- UPDATE Methods ----------------//

        /// <summary>
        /// Updates an existing schedule in the database.
        /// </summary>
        /// <param name="id">The ID of the existing schedule.</param>
        /// <param name="schedule">The updated schedule.</param>
        /// <returns>An action result.</returns>
        [HttpPut("Update/{id}")]
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

        //---------------- DELETE Methods ----------------//

        /// <summary>
        /// Removes a schedule from the database.
        /// </summary>
        /// <param name="id">The ID of the schedule.</param>
        /// <returns>An action result.</returns>
        [HttpDelete("Delete/{id}")]
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

        /// <summary>
        /// Removes all of a user's schedules from the database.
        /// </summary>
        /// <param name="userID">The ID of the user.</param>
        /// <returns>An action result.</returns>
        [HttpDelete("DeleteAll/{userID}")]
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
