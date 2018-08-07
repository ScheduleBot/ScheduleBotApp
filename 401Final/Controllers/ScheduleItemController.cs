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

        [HttpGet(Name = "GetScheduleItem")]
        [Route("Get/{id}")]
        public ActionResult<ScheduleItem> Get(int id)
        {
            var item = _context.GetScheduleItemByID(id);
            if (item is null) return NotFound();
            else return item;
        }

        [HttpGet]
        [Route("GetAll/{scheduleID}")]
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

        [HttpGet]
        [Route("Today/{scheduleID}")]
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

        [HttpGet]
        [Route("ThreeDay/{scheduleID}")]
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

        [HttpGet]
        [Route("Week/{scheduleID}")]
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
        [Route("New")]
        public IActionResult New([FromBody]ScheduleItem scheduleItem)
        {
            if (ModelState.IsValid)
            {
                _context.CreateScheduleItem(scheduleItem);
                return CreatedAtRoute("GetScheduleItem", new { id = scheduleItem.ID }, scheduleItem);
            }
            else return BadRequest();

        }

        [HttpPut]
        [Route("Update/{id}")]
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

        [HttpDelete]
        [Route("Delete/{id}")]
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

        [HttpDelete]
        [Route("DeleteAll/{scheduleID}")]
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
