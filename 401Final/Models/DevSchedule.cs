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
        public void CreateSchedule(Schedule schedule)
        {
             _context.Schedules.Add(schedule);
             _context.SaveChanges();
        }

        /// <summary>
        /// Adds a new schedule item to the database.
        /// </summary>
        /// <param name="scheduleItem">The schedule item being added.</param>
        public void CreateScheduleItem(ScheduleItem scheduleItem)
        {
             _context.ScheduleItems.Add(scheduleItem);
             _context.SaveChanges();
        }

        //---------------- READ Methods ----------------//

        /// <summary>
        /// Gets all items in a schedule.
        /// </summary>
        /// <param name="scheduleId">The ID of the schedule being queried.</param>
        /// <returns>A list of schedule items.</returns>
        public List<ScheduleItem> GetAllScheduleItems(int scheduleId)
        {
            if (GetScheduleByID(scheduleId) is null) throw new KeyNotFoundException();
            var items = _context.ScheduleItems.Where(x => x.ScheduleID == scheduleId);
            if (items.Any()) return items.ToList();
            else return null;
        }

        /// <summary>
        /// Gets a schedule by its ID.
        /// </summary>
        /// <param name="id">The ID being searched for.</param>
        /// <returns>A schedule.</returns>
        public Schedule GetScheduleByID(int id)
        {
            try
            {
                return _context.Schedules.Single(x => x.ID == id);
            }
            catch
            {
                return null;
            }
            
        }

        /// <summary>
        /// Gets a schedule item by its ID.
        /// </summary>
        /// <param name="id">The ID being searched for.</param>
        /// <returns>A schedule item.</returns>
        public ScheduleItem GetScheduleItemByID(int id)
        {
            try
            {
                return _context.ScheduleItems.Single(x => x.ID == id);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets all items in a schedule for a given day.
        /// </summary>
        /// <param name="scheduleId">The ID of the schedule being queried.</param>
        /// <param name="day">The date for the query.</param>
        /// <returns>A list of schedule items.</returns>
        public List<ScheduleItem> GetScheduleItemsByDay(int scheduleId, DateTime day)
        {
            List<ScheduleItem> items = GetAllScheduleItems(scheduleId);
            if (items is null) return null;
            else
            {
                List<ScheduleItem> todaysItems = new List<ScheduleItem>();
                foreach (ScheduleItem item in items)
                {
                    if (item.StartTime.Date == day.Date) todaysItems.Add(item);
                    else if (((byte)item.Days & (1 << (int)day.DayOfWeek)) != 0)
                    {
                        ScheduleItem newItem = new ScheduleItem()
                        {
                            Length = item.Length,
                            Description = item.Description,
                            Title = item.Title,
                            StartTime = new DateTime(day.Year, day.Month, day.Day,
                                item.StartTime.Hour, item.StartTime.Minute, item.StartTime.Second)
                        };
                        todaysItems.Add(newItem);
                    }
                }
                if (todaysItems.Any())
                {
                    todaysItems.Sort((item1, item2) => item1.StartTime.CompareTo(item2.StartTime));
                    return todaysItems;
                }
                else return null;
            }
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

        public List<ScheduleItem> Get3DayScheduleItems(int scheduleId, DateTime startDay)
        {
            return GetNDayScheduleItems(3, scheduleId, startDay);
        }

        /// <summary>
        /// Gets all items in a schedule in a 7-day window.
        /// </summary>
        /// <param name="scheduleId">The ID of the schedule being queried.</param>
        /// <param name="startDay">The start date for the query.</param>
        /// <returns>A list of schedule items.</returns>
        public List<ScheduleItem> GetWeeklyScheduleItems(int scheduleId, DateTime startDay)
        {
            return GetNDayScheduleItems(7, scheduleId, startDay);
        }

        //---------------- UPDATE Methods ----------------//

        /// <summary>
        /// Updates a schedule in the database
        /// </summary>
        /// <param name="id">The ID of the schedule.</param>
        /// <param name="schedule">The updated schedule.</param>
        public void UpdateSchedule(int id, Schedule schedule)
        {
            Schedule dbSchedule = GetScheduleByID(id);
            if (dbSchedule is null) throw new KeyNotFoundException();
            else
            {
                dbSchedule.Title = schedule.Title;
                _context.Update(dbSchedule);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Updates a schedule item in the database.
        /// </summary>
        /// <param name="id">The ID of the schedule item.</param>
        /// <param name="scheduleItem">The updated schedule item.</param>
        public void UpdateScheduleItem(int id, ScheduleItem scheduleItem)
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
                _context.SaveChanges();
            }
        }

        //---------------- DELETE Methods ----------------//

        /// <summary>
        /// Deletes all schedule items belonging to a schedule.
        /// </summary>
        /// <param name="scheduleId">The schedule's ID.</param>
        public void DeleteAllItemsInSchedule(int scheduleId)
        {
            List<ScheduleItem> scheduleItems = GetAllScheduleItems(scheduleId);
            if (scheduleItems is null) return;
            else
            {
                _context.ScheduleItems.RemoveRange(scheduleItems);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes all schedules belonging to a user.
        /// </summary>
        /// <param name="user">The user's ID.</param>
        public void DeleteAllUserSchedules(string user)
        {
            List<Schedule> schedules = GetUserSchedules(user);
            foreach (Schedule schedule in schedules)
            {
                DeleteAllItemsInSchedule(schedule.ID);
            }
            _context.Schedules.RemoveRange(schedules);
            _context.SaveChanges();
        }

        /// <summary>
        /// Deletes a schedule from the database.
        /// </summary>
        /// <param name="schedule">The schedule being deleted.</param>
        public void DeleteSchedule(Schedule schedule)
        {
            _context.Schedules.Remove(schedule);
            _context.SaveChanges();
        }

        /// <summary>
        /// Deletes a schedule item from the database.
        /// </summary>
        /// <param name="scheduleItem">The item being deleted.</param>
        public void DeleteScheduleItem(ScheduleItem scheduleItem)
        {
            _context.ScheduleItems.Remove(scheduleItem);
            _context.SaveChanges();
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
                if (now > endTime && item.Days == 0) DeleteScheduleItem(item);
            }
        }

        //---------------- HELPER METHODS ----------------//

        private List<ScheduleItem> GetNDayScheduleItems(int n, int scheduleId, DateTime startDay)
        {
            List<ScheduleItem> result = new List<ScheduleItem>(), dailyItems;
            for (int i = 0; i < n; i++)
            {
                dailyItems = GetScheduleItemsByDay(scheduleId, startDay.AddDays(i));
                if (dailyItems != null) result.AddRange(dailyItems);
            }
            if (result.Any()) return result;
            else return null;
        }
    }
}
