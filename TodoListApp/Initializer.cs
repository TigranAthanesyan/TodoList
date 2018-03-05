using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                Todo buyAccumulator = new Todo
                {
                    PersonId = tigran.PersonId,
                    What = "Buy an accumulator",
                    DeadLine = DateTime.Parse("2018/03/04")
                };
                context.TodoList.Add(buyAccumulator);
                context.SaveChanges();
            }
        }
    }
}
