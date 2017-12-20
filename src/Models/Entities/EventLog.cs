using System;

namespace Models.Entities {
    public class EventLog {
        public int ID { get; set; }
        public int EventID { get; set; }
        public string LogLevel { get; set; }
        public string Message { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}