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
            //arrange
            ScheduleDBContext context = MakeContext("ChatLogTest");

            ChatLog newLog = new ChatLog
            {
                TimeStamp = DateTime.Now,
                Chat = "This test string."
            };

            //act
            context.Add(newLog);
            context.SaveChanges();

            var testResult = from x in context.ChatLogs
                             select x;

            //assert
            Assert.NotNull(testResult);

            //reset
            ResetContext(context);
        }

        /// <summary>
        /// method to test that invalid JSON data will be accepted into database
        /// </summary>
        [Fact]
        public void CanInsertMalware()
        {
            //arrange
            ScheduleDBContext context = MakeContext("ChatLogTest2");

            ChatLog newLog = new ChatLog
            {
                TimeStamp = DateTime.Now,
                Chat = "{'chat': '{insert json malware here}'}"
            };

            //act
            context.Add(newLog);
            context.SaveChanges();

            var testResult = from x in context.ChatLogs
                             select x;

            //assert
            Assert.NotNull(testResult);

            //reset
            ResetContext(context);
        }

    }
}
