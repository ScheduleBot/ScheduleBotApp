using Final401.Data;
using Final401.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final401.Models
{
    public class DevSchedule : ISchedule
    {
        private ScheduleDBContext _context;

        public DevSchedule(ScheduleDBContext context)
        {
            _context = context;
        }

        //---------------- CREATE Methods ----------------//

        /// <summary>
        /// Adds a new schedule to the database.
        /// </summary>
        /// <param name="schedule">The schedule being added.</param>
        public async void CreateSchedule(Schedule schedule)
        {
            await _context.Schedules.AddAsync(schedule);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Adds a new schedule item to the database.
        /// </summary>
        /// <param name="scheduleItem">The schedule item being added.</param>
        public async void CreateScheduleItem(ScheduleItem scheduleItem)
        {
            await _context.ScheduleItems.AddAsync(scheduleItem);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Creates a duplicate of an existing schedule item.
        /// </summary>
        /// <param name="scheduleItem">The schedule item being duplicated</param>
        public void CreateRepeatScheduleItem(ScheduleItem scheduleItem)
        {
            if ((byte)scheduleItem.Days == 0) return;
            byte weekday;
            DateTime time = scheduleItem.StartTime;
            for (int i = 0; i < 7; i++)
            {
                time = time.AddDays(1);
                weekday = (byte)scheduleItem.StartTime.DayOfWeek;
                if (((byte)scheduleItem.Days & (1 << weekday)) != 0)
                {
                    ScheduleItem newItem = new ScheduleItem
                    {
                        Days = scheduleItem.Days,
                        Description = scheduleItem.Description,
                        Length = scheduleItem.Length,
                        ScheduleID = scheduleItem.ID,
                        StartTime = time,
                        Title = scheduleItem.Title
                    };
                    CreateScheduleItem(newItem);
                    break;
                }
            }
        }

        //---------------- READ Methods ----------------//

        /// <summary>
        /// Gets all items in a schedule.
        /// </summary>
        /// <param name="scheduleId">The ID of the schedule being queried.</param>
        /// <returns>A list of schedule items.</returns>
        public List<ScheduleItem> GetAllScheduleItems(int scheduleId)
        {
            var items = _context.ScheduleItems.Where(x => x.ScheduleID == scheduleId);
            if (items.Any()) return items.ToList();
            else throw new KeyNotFoundException();
        }

        /// <summary>
        /// Gets a schedule by its ID.
        /// </summary>
        /// <param name="id">The ID being searched for.</param>
        /// <returns>A schedule.</returns>
        public Schedule GetScheduleByID(int id)
        {
            return _context.Schedules.Single(x => x.ID == id);
        }

        /// <summary>
        /// Gets a schedule item by its ID.
        /// </summary>
        /// <param name="id">The ID being searched for.</param>
        /// <returns>A schedule item.</returns>
        public ScheduleItem GetScheduleItemByID(int id)
        {
            return _context.ScheduleItems.Single(x => x.ID == id);
        }

        /// <summary>
        /// Gets all items in a schedule for a given day.
        /// </summary>
        /// <param name="scheduleId">The ID of the schedule being queried.</param>
        /// <param name="day">The date for the query.</param>
        /// <returns>A list of schedule items.</returns>
        public List<ScheduleItem> GetScheduleItemsByDay(int scheduleId, DateTime day)
        {
            var items = GetAllScheduleItems(scheduleId).Where(x => x.StartTime.Date == day.Date);
            if (items.Any()) return items.ToList();
            else return null;
        }

        /// <summary>
        /// Gets all of a user's schedules.
        /// </summary>
        /// <param name="user">The user's ID.</param>
        /// <returns>A list of schedules.</returns>
        public List<Schedule> GetUserSchedules(string user)
        {
            var schedules = _context.Schedules.Where(x => x.UserID == user);
            if (schedules.Any()) return schedules.ToList();
            else throw new KeyNotFoundException();
        }

        /// <summary>
        /// Gets all items in a schedule in a 7-day window.
        /// </summary>
        /// <param name="scheduleId">The ID of the schedule being queried.</param>
        /// <param name="startDay">The start date for the query.</param>
        /// <returns>A list of schedule items.</returns>
        public List<ScheduleItem> GetWeeklyScheduleItems(int scheduleId, DateTime startDay)
        {
            var items = GetAllScheduleItems(scheduleId)
                .Where(x => x.StartTime.Date >= startDay.Date && x.StartTime.Date <= startDay.AddDays(7).Date);
            if (items.Any()) return items.ToList();
            else return null;
        }

        //---------------- UPDATE Methods ----------------//

        /// <summary>
        /// Updates a schedule in the database
        /// </summary>
        /// <param name="id">The ID of the schedule.</param>
        /// <param name="schedule">The updated schedule.</param>
        public async void UpdateSchedule(int id, Schedule schedule)
        {
            Schedule dbSchedule = GetScheduleByID(id);
            if (dbSchedule is null) throw new KeyNotFoundException();
            else
            {
                dbSchedule.Title = schedule.Title;
                _context.Update(dbSchedule);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Updates a schedule item in the database.
        /// </summary>
        /// <param name="id">The ID of the schedule item.</param>
        /// <param name="scheduleItem">The updated schedule item.</param>
        public async void UpdateScheduleItem(int id, ScheduleItem scheduleItem)
        {
            ScheduleItem dbScheduleItem = GetScheduleItemByID(id);
            if (dbScheduleItem is null) throw new KeyNotFoundException();
            else
            {
                dbScheduleItem.StartTime = scheduleItem.StartTime;
                dbScheduleItem.Days = scheduleItem.Days;
                dbScheduleItem.Description = scheduleItem.Description;
                dbScheduleItem.Length = scheduleItem.Length;
                dbScheduleItem.StartTime = scheduleItem.StartTime;
                dbScheduleItem.Title = scheduleItem.Title;
                _context.Update(dbScheduleItem);
                await _context.SaveChangesAsync();
            }
        }

        //---------------- DELETE Methods ----------------//

        /// <summary>
        /// Deletes all schedule items belonging to a schedule.
        /// </summary>
        /// <param name="scheduleId">The schedule's ID.</param>
        public async void DeleteAllItemsInSchedule(int scheduleId)
        {
            List<ScheduleItem> scheduleItems = GetAllScheduleItems(scheduleId);
            _context.ScheduleItems.RemoveRange(scheduleItems);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes all schedules belonging to a user.
        /// </summary>
        /// <param name="user">The user's ID.</param>
        public async void DeleteAllUserSchedules(string user)
        {
            List<Schedule> schedules = GetUserSchedules(user);
            foreach (Schedule schedule in schedules)
            {
                DeleteAllItemsInSchedule(schedule.ID);
            }
            _context.Schedules.RemoveRange(schedules);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a schedule from the database.
        /// </summary>
        /// <param name="schedule">The schedule being deleted.</param>
        public async void DeleteSchedule(Schedule schedule)
        {
            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a schedule item from the database.
        /// </summary>
        /// <param name="scheduleItem">The item being deleted.</param>
        public async void DeleteScheduleItem(ScheduleItem scheduleItem)
        {
            _context.ScheduleItems.Remove(scheduleItem);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes all expired schedule items.
        /// </summary>
        /// <param name="scheduleId">The schedule being cleaned.</param>
        public void DeleteOldScheduleItems(int scheduleId)
        {
            List<ScheduleItem> items = GetAllScheduleItems(scheduleId);
            DateTime now = DateTime.Now;
            DateTime endTime;
            foreach (ScheduleItem item in items)
            {
                endTime = item.StartTime.Add(item.Length);
                CreateRepeatScheduleItem(item);
                if (now > endTime) DeleteScheduleItem(item);
            }
        }
    }
}
