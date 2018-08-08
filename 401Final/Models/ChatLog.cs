using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final401.Models
{
    public class ChatLog
    {
        public int? ID { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Chat { get; set; }
        
    }
}
