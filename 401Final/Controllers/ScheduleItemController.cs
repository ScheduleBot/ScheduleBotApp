using Final401.Models;
using Final401.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final401.Controllers
{
    [Route("API/ScheduleItem")]
    public class ScheduleItemController : Controller
    {
        private readonly ISchedule _context;

        public ScheduleItemController(ISchedule context)
        {
            _context = context;
        }

        //---------------- READ Methods ----------------//

        /// <summary>
        /// Gets a schedule item by its ID.
        /// </summary>
        /// <param name="id">The ID being searched for.</param>
        /// <returns>A schedule item.</returns>
        [HttpGet("Get/{id}", Name = "GetScheduleItem")]
        public ActionResult<ScheduleItem> Get(int id)
        {
            var item = _context.GetScheduleItemByID(id);
            if (item is null) return NotFound();
            else return item;
        }

        /// <summary>
        /// Gets all items in a schedule.
        /// </summary>
        /// <param name="scheduleID">The ID of the schedule.</param>
        /// <returns>A list of schedule items.</returns>
        [HttpGet("GetAll/{scheduleID}")]
        public ActionResult<List<ScheduleItem>> GetAll(int scheduleID)
        {
            try
            {
                return _context.GetAllScheduleItems(scheduleID);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Gets all of the items in a schedule for today.
        /// </summary>
        /// <param name="scheduleID">The ID of the schedule.</param>
        /// <returns>A list of schedule items.</returns>
        [HttpGet("Today/{scheduleID}")]
        public ActionResult<List<ScheduleItem>> Today(int scheduleID)
        {
            try
            {
                return _context.GetScheduleItemsByDay(scheduleID, DateTime.Now);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Gets all of the items in a schedule for the next three days.
        /// </summary>
        /// <param name="scheduleID">The ID of the schedule.</param>
        /// <returns>A list of schedule items.</returns>
        [HttpGet("ThreeDay/{scheduleID}")]
        public ActionResult<List<ScheduleItem>> ThreeDay(int scheduleID)
        {
            try
            {
                return _context.Get3DayScheduleItems(scheduleID, DateTime.Now);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Gets all of the items in a schedule for the next week.
        /// </summary>
        /// <param name="scheduleID">The ID of the schedule.</param>
        /// <returns>A list of schedule items.</returns>
        [HttpGet("Week/{scheduleID}")]
        public ActionResult<List<ScheduleItem>> Week(int scheduleID)
        {
            try
            {
                return _context.GetWeeklyScheduleItems(scheduleID, DateTime.Now);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        //---------------- CREATE Methods ----------------//

        /// <summary>
        /// Adds a new schedule item to the database.
        /// </summary>
        /// <param name="scheduleItem">The schedule item being added.</param>
        /// <returns>An action result.</returns>
        [HttpPost("New")]
        public IActionResult New([FromBody]ScheduleItem scheduleItem)
        {
            if (ModelState.IsValid)
            {
                _context.CreateScheduleItem(scheduleItem);
                return CreatedAtRoute("GetScheduleItem", new { id = scheduleItem.ID }, scheduleItem);
            }
            else return BadRequest();

        }

        //---------------- UPDATE Methods ----------------//

        /// <summary>
        /// Updates an existing schedule item in the database.
        /// </summary>
        /// <param name="id">The ID of the existing schedule item.</param>
        /// <param name="scheduleItem">The updated schedule item.</param>
        /// <returns>An action result.</returns>
        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, [FromBody]ScheduleItem scheduleItem)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.UpdateScheduleItem(id, scheduleItem);
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
        /// Removes a schedule item from the database.
        /// </summary>
        /// <param name="id">The ID of the schedule item.</param>
        /// <returns>An action result.</returns>
        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var item = _context.GetScheduleItemByID(id);
            if (item is null) return NotFound();
            else
            {
                _context.DeleteScheduleItem(item);
                return NoContent();
            }
        }

        /// <summary>
        /// Removes all items in a schedule from the database.
        /// </summary>
        /// <param name="scheduleID">The ID of the schedule.</param>
        /// <returns></returns>
        [HttpDelete("DeleteAll/{scheduleID}")]
        public IActionResult DeleteAll(int scheduleID)
        {
            try
            {
                _context.DeleteAllItemsInSchedule(scheduleID);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
