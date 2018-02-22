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
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List(string what = default(string), int time = -1)
        {
            if (what != default(string) && time != -1)
            {
                foreach (Todo todo in Startup.TodoList)
                {
                    if (todo.What == what && todo.Time == time)
                    {
                        Startup.TodoList.Remove(todo);
                        break;
                    }
                }
            }

            return View(Startup.TodoList);
        }

        [HttpGet("home/add")]
        public IActionResult Add(string what, string time)
        {
            int ntime;
            if (what != default(string) && time != default(string) && int.TryParse(time, out ntime))
            {
                Todo todo = new Todo(what, ntime);
                Startup.TodoList.Add(todo);
            }
            return Redirect("List");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
