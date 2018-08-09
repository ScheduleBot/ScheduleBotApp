using Final401.Data;
using Final401.Models;
using System.Linq;
using System;
using Xunit;
using static ScheduleAPITests.TestHelpers;
using System.Collections.Generic;
using Final401.Models.Enum;

namespace ScheduleAPITests
{
    public class DevScheduleTests
    {
        /*--------------------------
        | Tests for CreateSchedule |
        --------------------------*/
        [Fact]
        public void CanCreateSchedule()
        {
            ScheduleDBContext context = MakeContext("devscheduletest1");
            DevSchedule devSchedule = new DevSchedule(context);
            Schedule schedule = new Schedule
            {
                UserID = "testUser",
                Title = "test"
            };
            devSchedule.CreateSchedule(schedule);
            Schedule test = (from s in context.Schedules
                       where s.Title == "test"
                       select s).First();
            Assert.NotNull(test);
            ResetContext(context);
        }

        /*------------------------------
        | Tests for CreateScheduleItem |
        ------------------------------*/
        [Fact]
        public void CanCreateScheduleItem()
        {
            ScheduleDBContext context = MakeContext("devscheduletest2");
            DevSchedule devSchedule = new DevSchedule(context);
            ScheduleItem scheduleItem = new ScheduleItem
            {
                Title = "test",
                StartTime = DateTime.Now,
                Length = new TimeSpan(0, 0, 0),
                Days = 0,
                Description = "test item for testing"
            };
            devSchedule.CreateScheduleItem(scheduleItem);
            ScheduleItem test = (from i in context.ScheduleItems
                                 where i.Title == "test"
                                 select i).First();
            Assert.NotNull(test);
            ResetContext(context);
        }

        /*-------------------------------
        | Tests for GetAllScheduleItems |
        -------------------------------*/
        [Fact]
        public void CanGetAllScheduleItems()
        {
            ScheduleDBContext context = MakeContext("devscheduletest3");
            Schedule schedule = new Schedule
            {
                UserID = "testUser",
                Title = "test"
            };
            context.Schedules.Add(schedule);
            context.SaveChanges();
            context.ScheduleItems.AddRange(
                new ScheduleItem
                {
                    Title = "test",
                    StartTime = DateTime.Now,
                    Length = new TimeSpan(0, 0, 0),
                    Days = 0,
                    Description = "test item for testing",
                    ScheduleID = schedule.ID
                },

                new ScheduleItem
                {
                    Title = "test",
                    StartTime = DateTime.Now.AddDays(2),
                    Length = new TimeSpan(1, 30, 0),
                    Days = (DayFlags)127,
                    Description = "test item for testing",
                    ScheduleID = schedule.ID
                });
            context.SaveChanges();
            DevSchedule devSchedule = new DevSchedule(context);
            var result = devSchedule.GetAllScheduleItems(schedule.ID);
            Assert.Equal(2, result.Count);
            ResetContext(context);
        }

        [Fact]
        public void GetAllScheduleItemsInEmptySchedule()
        {
            ScheduleDBContext context = MakeContext("devscheduletest4");
            Schedule schedule = new Schedule
            {
                UserID = "testUser",
                Title = "test"
            };
            context.Schedules.Add(schedule);
            context.SaveChanges();
            DevSchedule devSchedule = new DevSchedule(context);
            var result = devSchedule.GetAllScheduleItems(schedule.ID);
            Assert.Null(result);
            ResetContext(context);
        }

        [Fact]
        public void GetAllScheduleItemsOnNonExistantSchedule()
        {
            ScheduleDBContext context = MakeContext("devscheduletest5");
            DevSchedule devSchedule = new DevSchedule(context);
            Assert.Throws<KeyNotFoundException>(() => devSchedule.GetAllScheduleItems(1));
        }

        /*---------------------------
        | Tests for GetScheduleByID |
        ---------------------------*/
        [Fact]
        public void CanGetScheduleByID()
        {
            ScheduleDBContext context = MakeContext("devscheduletest6");
            Schedule schedule = new Schedule
            {
                UserID = "testUser",
                Title = "test"
            };
            context.Schedules.Add(schedule);
            context.SaveChanges();
            DevSchedule devSchedule = new DevSchedule(context);
            var result = devSchedule.GetScheduleByID(schedule.ID);
            Assert.Equal(schedule, result);
            ResetContext(context);
        }

        [Fact]
        public void GetScheduleByIDOnNonExistantSchedule()
        {
            ScheduleDBContext context = MakeContext("devscheduletest7");
            DevSchedule devSchedule = new DevSchedule(context);
            var result = devSchedule.GetScheduleByID(1);
            Assert.Null(result);
        }

        /*-------------------------------
        | Tests for GetScheduleItemByID |
        -------------------------------*/
        [Fact]
        public void CanGetScheduleItemByID()
        {
            ScheduleDBContext context = MakeContext("devscheduletest8");
            ScheduleItem item = new ScheduleItem
            {
                Title = "test",
                StartTime = DateTime.Now,
                Length = new TimeSpan(0, 0, 0),
                Days = 0,
                Description = "test item for testing",
            };
            context.ScheduleItems.Add(item);
            context.SaveChanges();
            DevSchedule devSchedule = new DevSchedule(context);
            var result = devSchedule.GetScheduleItemByID(item.ID);
            Assert.Equal(item, result);
            ResetContext(context);
        }

        [Fact]
        public void GetScheduleItemByIDOnNonExistantScheduleItem()
        {
            ScheduleDBContext context = MakeContext("devscheduletest9");
            DevSchedule devSchedule = new DevSchedule(context);
            var result = devSchedule.GetScheduleItemByID(1);
            Assert.Null(result);
        }

        /*---------------------------------
        | Tests for GetScheduleItemsByDay |
        ---------------------------------*/
        [Fact]
        public void CanGetScheduleItemsByDay()
        {
            ScheduleDBContext context = MakeContext("devscheduletest10");
            Schedule schedule = new Schedule
            {
                UserID = "testUser",
                Title = "test"
            };
            context.Schedules.Add(schedule);
            context.SaveChanges();
            ScheduleItem item = new ScheduleItem
            {
                Title = "test",
                StartTime = DateTime.Now,
                Length = new TimeSpan(1, 30, 0),
                Days = (DayFlags)127,
                Description = "test item for testing",
                ScheduleID = schedule.ID
            };
            context.ScheduleItems.Add(item);
            context.SaveChanges();
            DevSchedule devSchedule = new DevSchedule(context);
            var result = devSchedule.GetScheduleItemsByDay(schedule.ID, DateTime.Now);
            Assert.Single(result);
            ResetContext(context);
        }

        [Fact]
        public void GetScheduleItemsByDayNoItemsFound()
        {
            ScheduleDBContext context = MakeContext("devscheduletest11");
            Schedule schedule = new Schedule
            {
                UserID = "testUser",
                Title = "test"
            };
            context.Schedules.Add(schedule);
            context.SaveChanges();
            ScheduleItem item = new ScheduleItem
            {
                Title = "test",
                StartTime = DateTime.Now.AddDays(1),
                Length = new TimeSpan(1, 30, 0),
                Days = 0,
                Description = "test item for testing",
                ScheduleID = schedule.ID
            };
            context.ScheduleItems.Add(item);
            context.SaveChanges();
            DevSchedule devSchedule = new DevSchedule(context);
            var result = devSchedule.GetScheduleItemsByDay(schedule.ID, DateTime.Now);
            Assert.Null(result);
            ResetContext(context);
        }

        [Fact]
        public void GetScheduleItemsByDayNoScheduleFound()
        {
            ScheduleDBContext context = MakeContext("devscheduletest12");
            DevSchedule devSchedule = new DevSchedule(context);
            Assert.Throws<KeyNotFoundException>(() => devSchedule.GetScheduleItemsByDay(1, DateTime.Now));
        }

        /*----------------------------
        | Tests for GetUserSchedules |
        ----------------------------*/
        [Fact]
        public void CanGetUserSchedules()
        {
            ScheduleDBContext context = MakeContext("devscheduletest13");
            context.Schedules.AddRange(
                new Schedule
                {
                    Title = "My first schedule",
                    UserID = "abc123"
                },
                new Schedule
                {
                    Title = "My second schedule",
                    UserID = "abc123"
                });
            context.SaveChanges();
            DevSchedule devSchedule = new DevSchedule(context);
            var result = devSchedule.GetUserSchedules("abc123");
            Assert.Equal(2, result.Count);
            ResetContext(context);
        }

        [Fact]
        public void GetUserSchedulesNoSchedulesFound()
        {
            ScheduleDBContext context = MakeContext("devscheduletest14");
            DevSchedule devSchedule = new DevSchedule(context);
            Assert.Throws<KeyNotFoundException>(() => devSchedule.GetUserSchedules("me"));
        }

        /*--------------------------------
        | Tests for Get3DayScheduleItems |
        --------------------------------*/
        [Fact]
        public void CanGet3DayScheduleItems()
        {
            ScheduleDBContext context = MakeContext("devscheduletest15");
            Schedule schedule = new Schedule
            {
                UserID = "testUser",
                Title = "test"
            };
            context.Schedules.Add(schedule);
            context.SaveChanges();
            ScheduleItem item = new ScheduleItem
            {
                Title = "test",
                StartTime = DateTime.Now,
                Length = new TimeSpan(1, 30, 0),
                Days = (DayFlags)127,
                Description = "test item for testing",
                ScheduleID = schedule.ID
            };
            context.ScheduleItems.Add(item);
            context.SaveChanges();
            DevSchedule devSchedule = new DevSchedule(context);
            var result = devSchedule.Get3DayScheduleItems(schedule.ID, DateTime.Now);
            Assert.Equal(3, result.Count);
            ResetContext(context);
        }

        [Fact]
        public void Get3DayScheduleItemsNoItemsFound()
        {
            ScheduleDBContext context = MakeContext("devscheduletest16");
            Schedule schedule = new Schedule
            {
                UserID = "testUser",
                Title = "test"
            };
            context.Schedules.Add(schedule);
            context.SaveChanges();
            ScheduleItem item = new ScheduleItem
            {
                Title = "test",
                StartTime = DateTime.Now,
                Length = new TimeSpan(1, 30, 0),
                Days = 0,
                Description = "test item for testing",
                ScheduleID = schedule.ID
            };
            context.ScheduleItems.Add(item);
            context.SaveChanges();
            DevSchedule devSchedule = new DevSchedule(context);
            var result = devSchedule.Get3DayScheduleItems(schedule.ID, DateTime.Now.AddDays(1));
            Assert.Null(result);
            ResetContext(context);
        }

        [Fact]
        public void Get3DaySchedleItemsNoScheduleFound()
        {
            ScheduleDBContext context = MakeContext("devscheduletest17");
            DevSchedule devSchedule = new DevSchedule(context);
            Assert.Throws<KeyNotFoundException>(() => devSchedule.Get3DayScheduleItems(1, DateTime.Now));
        }

        /*----------------------------------
        | Tests for GetWeeklyScheduleItems |
        ----------------------------------*/
        [Fact]
        public void CanGetWeeklyScheduleItems()
        {
            ScheduleDBContext context = MakeContext("devscheduletest18");
            Schedule schedule = new Schedule
            {
                UserID = "testUser",
                Title = "test"
            };
            context.Schedules.Add(schedule);
            context.SaveChanges();
            ScheduleItem item = new ScheduleItem
            {
                Title = "test",
                StartTime = DateTime.Now,
                Length = new TimeSpan(1, 30, 0),
                Days = (DayFlags)127,
                Description = "test item for testing",
                ScheduleID = schedule.ID
            };
            context.ScheduleItems.Add(item);
            context.SaveChanges();
            DevSchedule devSchedule = new DevSchedule(context);
            var result = devSchedule.GetWeeklyScheduleItems(schedule.ID, DateTime.Now);
            Assert.Equal(7, result.Count);
            ResetContext(context);
        }

        [Fact]
        public void GetWeeklyScheduleItemsNoItemsFound()
        {
            ScheduleDBContext context = MakeContext("devscheduletest19");
            Schedule schedule = new Schedule
            {
                UserID = "testUser",
                Title = "test"
            };
            context.Schedules.Add(schedule);
            context.SaveChanges();
            ScheduleItem item = new ScheduleItem
            {
                Title = "test",
                StartTime = DateTime.Now,
                Length = new TimeSpan(1, 30, 0),
                Days = 0,
                Description = "test item for testing",
                ScheduleID = schedule.ID
            };
            context.ScheduleItems.Add(item);
            context.SaveChanges();
            DevSchedule devSchedule = new DevSchedule(context);
            var result = devSchedule.GetWeeklyScheduleItems(schedule.ID, DateTime.Now.AddDays(1));
            Assert.Null(result);
            ResetContext(context);
        }

        [Fact]
        public void GetWeeklyScheduleItemsNoScheduleFound()
        {
            ScheduleDBContext context = MakeContext("devscheduletest20");
            DevSchedule devSchedule = new DevSchedule(context);
            Assert.Throws<KeyNotFoundException>(() => devSchedule.GetWeeklyScheduleItems(1, DateTime.Now));
        }

        /*--------------------------
        | Tests for UpdateSchedule |
        --------------------------*/
        [Fact]
        public void CanUpdateSchedule()
        {
            ScheduleDBContext context = MakeContext("devscheduletest21");
            Schedule schedule = new Schedule
            {
                UserID = "bob",
                Title = "bob's tod do list"
            };
            context.Schedules.Add(schedule);
            context.SaveChanges();
            DevSchedule devSchedule = new DevSchedule(context);
            Schedule updatedSchedule = new Schedule
            {
                Title = "bob's to do list"
            };
            devSchedule.UpdateSchedule(schedule.ID, updatedSchedule);
            Assert.Equal(updatedSchedule.Title, schedule.Title);
            ResetContext(context);
        }

        [Fact]
        public void UpdateScheduleScheduleNotFound()
        {
            ScheduleDBContext context = MakeContext("devscheduletest22");
            DevSchedule devSchedule = new DevSchedule(context);
            Assert.Throws<KeyNotFoundException>(() => devSchedule.UpdateSchedule(1, new Schedule()));
        }

        /*------------------------------
        | Tests for UpdateScheduleItem |
        ------------------------------*/
        [Fact]
        public void CanUpdateScheduleItem()
        {
            ScheduleDBContext context = MakeContext("devscheduletest23");
            ScheduleItem item = new ScheduleItem
            {
                Title = "mow the lawn",
                Description = "the grass is too long! time to mow it.",
                StartTime = new DateTime(2180, 8, 31),
                Length = new TimeSpan(2, 0, 0)
            };
            context.ScheduleItems.Add(item);
            context.SaveChanges();
            DevSchedule devSchedule = new DevSchedule(context);
            ScheduleItem updatedItem = new ScheduleItem
            {
                Title = item.Title,
                Description = item.Description,
                StartTime = new DateTime(2018, 8, 31),
                Length = item.Length
            };
            devSchedule.UpdateScheduleItem(item.ID, updatedItem);
            Assert.Equal(updatedItem.StartTime, item.StartTime);
            ResetContext(context);
        }

        [Fact]
        public void UpdateScheduleItemItemNotFound()
        {
            ScheduleDBContext context = MakeContext("devscheduletest24");
            DevSchedule devSchedule = new DevSchedule(context);
            Assert.Throws<KeyNotFoundException>(() => devSchedule.UpdateScheduleItem(1, new ScheduleItem()));
        }

        /*------------------------------------
        | Tests for DeleteAllItemsInSchedule |
        ------------------------------------*/
        [Fact]
        public void CanDeleteAllItemsInSchedule()
        {
            ScheduleDBContext context = MakeContext("devscheduletest25");
            Schedule schedule = new Schedule
            {
                UserID = "bob",
                Title = "bob's to do list"
            };
            context.Schedules.Add(schedule);
            context.SaveChanges();
            context.ScheduleItems.AddRange(
                new ScheduleItem
                {
                    Title = "mow the lawn",
                    Description = "the grass is too long! time to mow it",
                    StartTime = new DateTime(2018, 8, 31),
                    Length = new TimeSpan(2, 0, 0),
                    ScheduleID = schedule.ID
                },
                new ScheduleItem
                {
                    Title = "do the laundry",
                    Description = "it's laundry day",
                    StartTime = new DateTime(2018, 8, 11),
                    Length = new TimeSpan(0),
                    ScheduleID = schedule.ID,
                    Days = DayFlags.Saturday
                });
            context.SaveChanges();
            DevSchedule devSchedule = new DevSchedule(context);
            devSchedule.DeleteAllItemsInSchedule(schedule.ID);
            Assert.Empty(context.ScheduleItems);
            ResetContext(context);
        }

        [Fact]
        public void DeleteAllItemsInScheduleScheduleNotFound()
        {
            ScheduleDBContext context = MakeContext("devscheduletest26");
            DevSchedule devSchedule = new DevSchedule(context);
            Assert.Throws<KeyNotFoundException>(() => devSchedule.DeleteAllItemsInSchedule(1));
        }

        /*----------------------------------
        | Tests for DeleteAllUserSchedules |
        ----------------------------------*/
        [Fact]
        public void CanDeleteAllUserSchedules()
        {
            ScheduleDBContext context = MakeContext("devscheduletest27");
            context.Schedules.AddRange(
                new Schedule
                {
                    UserID = "bob",
                    Title = "bob's chores"
                },
                new Schedule
                {
                    UserID = "bob",
                    Title = "bob's homework"
                });
            context.SaveChanges();
            DevSchedule devSchedule = new DevSchedule(context);
            devSchedule.DeleteAllUserSchedules("bob");
            Assert.Empty(context.Schedules);
        }

        [Fact]
        public void DeleteAllUserSchedulesSchedulesNotFound()
        {
            ScheduleDBContext context = MakeContext("devscheduletest28");
            DevSchedule devSchedule = new DevSchedule(context);
            Assert.Throws<KeyNotFoundException>(() => devSchedule.DeleteAllUserSchedules("bob"));
        }

        /*-------------------------
        | Tests for DeleteSchedule|
        -------------------------*/
        [Fact]
        public void CanDeleteSchedule()
        {
            ScheduleDBContext context = MakeContext("devscheduletest29");
            Schedule schedule = new Schedule
            {
                UserID = "bob",
                Title = "bob's to do list"
            };
            context.Schedules.Add(schedule);
            context.SaveChanges();
            DevSchedule devSchedule = new DevSchedule(context);
            devSchedule.DeleteSchedule(schedule);
            Assert.Empty(context.Schedules);
        }

        /*------------------------------
        | Tests for DeleteScheduleItem |
        ------------------------------*/
        [Fact]
        public void CanDeleteScheduleItem()
        {
            ScheduleDBContext context = MakeContext("devscheduletest30");
            ScheduleItem item = new ScheduleItem
            {
                Title = "mow the lawn",
                Description = "the grass is too long! time to mow it",
                StartTime = new DateTime(2018, 8, 31),
                Length = new TimeSpan(2, 0, 0),
            };
            context.ScheduleItems.Add(item);
            context.SaveChanges();
            DevSchedule devSchedule = new DevSchedule(context);
            devSchedule.DeleteScheduleItem(item);
            Assert.Empty(context.ScheduleItems);
        }
    }
}
