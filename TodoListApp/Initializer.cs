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
                Person testUser = new Person { UserName = "TestUser", Password = "test" };
                context.People.Add(testUser);
                context.SaveChanges();
            }
            if (!context.TodoList.Any())
            {
                Person testUser = context.People.FirstOrDefault(p => p.UserName == "TestUser");
                Todo initialTodo = new Todo
                {
                    UserName = testUser.UserName,
                    What = "Do something good",
                    DeadLine = DateTime.Now
                };
                context.TodoList.Add(initialTodo);
                context.SaveChanges();
            }
        }
    }
}
