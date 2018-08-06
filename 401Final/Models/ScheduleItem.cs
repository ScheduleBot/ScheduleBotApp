using Final401.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Final401.Models
{
    public class ScheduleItem
    {
        public int ID { get; set; }
        public int ScheduleID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Length { get; set; }
        public DayFlags Days { get; set; } = 0;
    }
}
