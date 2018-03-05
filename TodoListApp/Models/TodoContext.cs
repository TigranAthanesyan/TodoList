using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                .HasKey(c => c.PersonId);
            modelBuilder.Entity<Todo>()
                .HasKey(c => new { c.PersonId, c.What });
        }
    }
}
