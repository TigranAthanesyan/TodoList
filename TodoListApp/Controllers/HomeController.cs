using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models;

namespace TodoListApp.Controllers
{
    public class HomeController : Controller
    {
        TodoContext db;
        public HomeController(TodoContext dbContext) { db = dbContext; }

        public IActionResult Index() { return View(); }

        public IActionResult List(string name)
        {
            var person = db.People.FirstOrDefault(p => p.Name == name);
            int personId = 0;
            if (person != null)
            {
                personId = person.PersonId;
            }
            else
            {
                Person newPerson = new Person { Name = name };
                db.People.Add(newPerson);
                db.SaveChanges();
                personId = newPerson.PersonId;
            }

            var todoList = db.TodoList.Where(t => t.PersonId == personId).ToList();
            todoList.Sort((t1, t2) =>
            {
                if (t1.DeadLine.Date < t2.DeadLine.Date) { return -1; };
                if (t1.DeadLine.Date > t2.DeadLine.Date) { return  1; };
                return 0;
            });
            ViewBag.PersonId = personId;
            ViewBag.Name = name;
            return View(todoList);
        }

        public IActionResult Add(Todo todo)
        {
            var person = db.People.FirstOrDefault(p => p.PersonId == todo.PersonId);
            if (person == null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Adding new node failed!! Reason: Person with PersonId ");
                sb.Append(todo.PersonId);
                sb.Append(" does not exist..");
                return RedirectToAction(nameof(this.Error), new { message = sb.ToString() });
            }

            var name = person.Name;
            if (todo.What == null)
            {
                return RedirectToAction(nameof(this.List), new { name = name });
            }

            try
            {
                db.TodoList.Add(todo);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Adding new node \"");
                sb.Append(todo.What);
                sb.Append("\" failed!! ");
                sb.Append("Reason: ");
                sb.Append(ex.Message);
                return RedirectToAction(nameof(this.Error), new { message = sb.ToString() });
            }

            return RedirectToAction(nameof(this.List), new { name = name });
        }

        public IActionResult Complete(Todo todo)
        {
            var person = db.People.FirstOrDefault(p => p.PersonId == todo.PersonId);
            if (person == null || todo.What == null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Finishing node failed!! Reason: ");
                if (person == null)
                {
                    sb.Append("Person with PersonId ");
                    sb.Append(todo.PersonId);
                    sb.Append(" does not exist..");
                }
                else
                {
                    sb.Append("Node is empty..");
                }
                return RedirectToAction(nameof(this.Error), new { message = sb.ToString() });
            }

            try
            {
                db.TodoList.Remove(todo);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Finishing node \"");
                sb.Append(todo.What);
                sb.Append("\" failed!! ");
                sb.Append("Reason: ");
                sb.Append(ex.Message);
                return RedirectToAction(nameof(this.Error), new { message = sb.ToString() });
            }
            var name = person.Name;
            return RedirectToAction(nameof(this.List), new { name = name });
        }

        public IActionResult Error(string message = null)
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Message = message
            });
        }
    }
}
