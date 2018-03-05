using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoListApp.Models
{
    public class Todo
    {
        /*public Todo(int personId, string what, DateTime deadLine)
        {
            PersonId = personId;
            What = what;
            DeadLine = deadLine;
        }*/

        public bool Equals(Todo other)
        {
            return 
                this.PersonId == other.PersonId &&
                this.What == other.What &&
                this.DeadLine == other.DeadLine;
        }

        public int PersonId { get; set; }
        public string What { get; set; }
        public DateTime DeadLine { get; set; }
    }
}
