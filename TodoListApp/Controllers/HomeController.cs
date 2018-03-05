using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models;

namespace TodoListApp.Controllers
{
    public class HomeController : Controller
    {
        TodoContext db;
        public HomeController(TodoContext dbContext) { db = dbContext; }

        public IActionResult Index() { return View(); }

        private IActionResult List(int id)
        {
            var todoList = db.TodoList.ToList();
            var retList = new List<Todo>();
            foreach (var todo in todoList)
            {
                if (todo.PersonId == id)
                {
                    retList.Add(todo);
                }
            }
            ViewBag.PersonId = id;
            return View(retList);
        }
        public IActionResult List(string name)
        {
            var people = db.People.ToList();
            int id = 0;
            foreach (var person in people)
            {
                if (person.Name == name)
                {
                    id = person.PersonId;
                    break;
                }
            }
            return List(id);
        }

        public IActionResult Add(Todo todo)
        {
            try
            {
                db.TodoList.Add(todo);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return RedirectToAction(nameof(this.List), new { id = todo.PersonId });
        }

        public IActionResult Complete(Todo todo)
        {
            try
            {
                db.Remove(db.TodoList.Single(t => t.What == todo.What && t.PersonId == todo.PersonId));
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
            return RedirectToAction(nameof(this.List), new { id = todo.PersonId });
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
