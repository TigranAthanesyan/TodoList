using System;

namespace TodoListApp.Models
{
    public class Todo
    {
        public string UserName { get; set; }
        public string What { get; set; }
        public DateTime DeadLine { get; set; }
        public DateTime ActualDate { get; set; }
    }
}
