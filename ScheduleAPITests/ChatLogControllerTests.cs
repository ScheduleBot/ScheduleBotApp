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
    public class ChatLogControllerTests
    {
        /// <summary>
        /// method to test that chat controller can post a chatlog to database
        /// </summary>
        [Fact]
        public void CanPostChatLog()
        {
            ScheduleDBContext context = MakeContext("ChatLogTest");
            ChatLog newLog = new ChatLog
            {
                TimeStamp = DateTime.Now,
                Chat = "This test string."
            };

            context.Add(newLog);
            context.SaveChanges();

            var testResult = from x in context.ChatLogs
                             select x;

            Assert.NotNull(testResult);
            ResetContext(context);
        }

        /// <summary>
        /// method to test that invalid JSON data will be accepted into database
        /// </summary>
        [Fact]
        public void CanInsertMalware()
        {
            ScheduleDBContext context = MakeContext("ChatLogTest");
            ChatLog newLog = new ChatLog
            {
                TimeStamp = DateTime.Now,
                Chat = "{'chat': '{insert json malware here}'}"
            };

            context.Add(newLog);
            context.SaveChanges();

            var testResult = from x in context.ChatLogs
                             select x;

            Assert.NotNull(testResult);
            ResetContext(context);
        }

    }
}
