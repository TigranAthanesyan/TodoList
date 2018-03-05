using System;

namespace TodoListApp.Models
{
    public class Todo
    {
        public int PersonId { get; set; }
        public string What { get; set; }
        public DateTime DeadLine { get; set; }
    }
}
