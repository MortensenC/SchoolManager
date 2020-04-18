using System;
using System.Collections.Generic;

namespace SchoolManager.WebUI.ViewModels
{
    public class EventVM
    {
        public Int64 id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public int? classroomId { get; set; }
        public string classroomName { get; set; }
        public string students { get; set; }
        public bool editable { get; set; }
    }
}