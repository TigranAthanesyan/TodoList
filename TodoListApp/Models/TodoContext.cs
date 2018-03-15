using Microsoft.EntityFrameworkCore;

namespace TodoListApp.Models
{
    public class TodoContext : DbContext
    {
        public DbSet<Person> People { get; set; }
        public DbSet<Todo> TodoList { get; set; }

        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .HasKey(person => person.UserName);
            modelBuilder.Entity<Todo>()
                .HasKey(todo => new { todo.UserName, todo.What });
        }
    }
}
