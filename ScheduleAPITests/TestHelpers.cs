using Final401.Data;
using Final401.Models;
using Final401.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleAPITests
{
    public class TestHelpers
    {
        public static ScheduleDBContext MakeContext(string dbName)
        {
            DbContextOptions<ScheduleDBContext> options =
                new DbContextOptionsBuilder<ScheduleDBContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            return new ScheduleDBContext(options);
        }

        public static void ResetContext(ScheduleDBContext context)
        {
            context.Schedules.RemoveRange(context.Schedules);
            context.ScheduleItems.RemoveRange(context.ScheduleItems);
            context.SaveChanges();
        }
    }
}
