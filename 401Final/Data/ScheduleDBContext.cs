using Final401.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final401.Data
{
    public class ScheduleDBContext : DbContext
    {
        public ScheduleDBContext(DbContextOptions<ScheduleDBContext> options) : base(options){}

        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ScheduleItem> ScheduleItems { get; set; }
    }
}
