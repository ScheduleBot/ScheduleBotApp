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

        [HttpGet("{id}")]
        public ActionResult<ScheduleItem> Get(int id)
        {
            var item = _context.GetScheduleItemByID(id);
            if (item is null) return NotFound();
            else return item;
        }

        [HttpGet("{scheduleID}")]
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

        [HttpGet("{scheduleID}")]
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

        [HttpGet("{scheduleID}")]
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

        [HttpPost]
        public IActionResult New([FromBody] ScheduleItem scheduleItem)
        {
            if (ModelState.IsValid)
            {
                _context.CreateScheduleItem(scheduleItem);
                return CreatedAtRoute("Get", new { id = scheduleItem.ID }, scheduleItem);
            }
            else return BadRequest();
        }

        [HttpPut]
        public IActionResult Update(int scheduleID, [FromBody] ScheduleItem scheduleItem)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.UpdateScheduleItem(scheduleID, scheduleItem);
                    return NoContent();
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
            }
            else return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var item = _context.GetScheduleItemByID(id);
                _context.DeleteScheduleItem(item);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{scheduleID}")]
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
