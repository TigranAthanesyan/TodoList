using System;
using System.Linq;
using TodoListApp.Models;

namespace TodoListApp
{
    public static class Initializer
    {
        public static void Initialize(TodoContext context)
        {
            if (!context.People.Any())
            {
                Person tigran = new Person { Name = "Tigran" };
                context.People.Add(tigran);
                context.SaveChanges();
            }
            if (!context.TodoList.Any())
            {
                Person tigran = context.People.ToList()[0];
                Todo dummy = new Todo
                {
                    PersonId = tigran.PersonId,
                    What = "Do something good",
                    DeadLine = DateTime.Now
                };
                context.TodoList.Add(dummy);
                context.SaveChanges();
            }
        }
    }
}
