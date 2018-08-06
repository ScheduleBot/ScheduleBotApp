using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final401.Models.Interfaces
{
    public interface ISchedule
    {
        // Schedule Methods
        void CreateSchedule(Schedule schedule);
        List<Schedule> GetUserSchedules(string user);
        Schedule GetScheduleByID(int id);
        void UpdateSchedule(int id, Schedule schedule);
        void DeleteSchedule(Schedule schedule);
        void DeleteAllUserSchedules(string user);

        // ScheduleItem Methods
        void CreateScheduleItem(ScheduleItem scheduleItem);
        void CreateRepeatScheduleItem(ScheduleItem scheduleItem);
        List<ScheduleItem> GetAllScheduleItems(int scheduleId);
        List<ScheduleItem> GetScheduleItemsByDay(int scheduleId, DateTime day);
        List<ScheduleItem> GetWeeklyScheduleItems(int scheduleId, DateTime startDay);
        ScheduleItem GetScheduleItemByID(int id);
        void UpdateScheduleItem(int id, ScheduleItem scheduleItem);
        void DeleteScheduleItem(ScheduleItem scheduleItem);
        void DeleteAllItemsInSchedule(int scheduleId);
        void DeleteOldScheduleItems(int scheduleId);
    }
}
