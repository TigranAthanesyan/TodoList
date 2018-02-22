using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoListApp
{
    public class Todo
    {
        public Todo(string i_what, int i_time)
        {
            Edit(i_what);
            Edit(i_time);
        }
        public void Edit(string i_what)
        {
            What = i_what;
        }
        public void Edit(int i_time)
        {
            Time = i_time;
        }
        public string What
        {
            get;
            private set;
        }
        public int Time
        {
            get;
            private set;
        }
    }
}
