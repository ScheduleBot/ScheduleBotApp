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
    public class HomeControllerTests
    {
        /// <summary>
        /// method to test that chat controller can retrieve a chatlog to database
        /// </summary>
        [Fact]
        public void CanRetrieveEntries()
        {
            //arrange
            ScheduleDBContext context = MakeContext("RecentActivityTest");

            ChatLog newLog = new ChatLog
            {
                TimeStamp = DateTime.Now,
                Chat = "This test string."
            };

            ChatLog newLog2 = new ChatLog
            {
                TimeStamp = DateTime.Now,
                Chat = "Another test string"
            };

            //act
            context.Add(newLog);
            context.Add(newLog2);
            context.SaveChanges();

            //code matching controller method
            var result = from x in context.ChatLogs.Take(20)
                         orderby x.TimeStamp descending
                         select x;

            //assert
            Assert.Equal(2, result.Count());

            //reset
            ResetContext(context);
        }
    }
}
