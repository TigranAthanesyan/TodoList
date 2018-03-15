using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Models;

namespace TodoListApp.Controllers
{
    [Route("Home")]
    public class HomeController : Controller
    {
        private readonly TodoContext _dbContext;

        public static string CookyUserName { get { return "user_name"; } }

        public static bool IsAllowableFunction(string path)
        {
            return path.EndsWith("Home/SignIn") ||
                   path.EndsWith("Home/SignUp") ||
                   path.EndsWith("Home/SignUpConfirmation") ||
                   path.EndsWith("Home/Error");
        }

        public HomeController(TodoContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("SignIn")]
        public IActionResult SignIn(string userName)
        {
            var usersList = _dbContext.People.ToList();
            var userNamesList = new List<string>();
            foreach(var person in usersList)
            {
                userNamesList.Add(person.UserName);
            }

            ViewBag.UserName = userName;
            return View(userNamesList);
        }

        [HttpPost("SignIn")]
        public IActionResult SignIn([FromForm]string userName, string password)
        {
            var person = _dbContext.People.FirstOrDefault(p => p.UserName == userName);
            if (person == null)
            {
                StringBuilder sb = new StringBuilder("Can not open account!! Reason: User with user name \"");
                sb.Append(userName);
                sb.Append("\" does not exist..");
                return RedirectToAction(nameof(this.Error), new { message = sb.ToString() });
            }
            if (person.Password != password)
            {
                StringBuilder sb = new StringBuilder("Can not open account ");
                sb.Append(userName);
                sb.Append("!! Reason: Password is incorrect..");
                return RedirectToAction(nameof(this.Error), new { message = sb.ToString() });
            }

            if (Request.Cookies.ContainsKey(CookyUserName))
            {
                Response.Cookies.Delete(CookyUserName);
            }
            Response.Cookies.Append(CookyUserName, userName);

            return RedirectToAction(nameof(this.List));
        }

        [HttpGet("SignUp")]
        public IActionResult SignUp() { return View(); }

        [HttpPost("SignUp")]
        public IActionResult SignUp([FromForm]string userName, string password, string passwordCopy)
        {
            userName = userName.Trim();
            if (string.IsNullOrEmpty(userName))
            {
                StringBuilder sb = new StringBuilder("Account creating failed!! Reason: User name is empty..");
                return RedirectToAction(nameof(this.Error), new { message = sb.ToString(), actionName = nameof(this.SignUp) });
            }
            if (_dbContext.People.Any(p => p.UserName == userName))
            {
                StringBuilder sb = new StringBuilder("Account creating failed!! Reason: User with User name ");
                sb.Append(userName);
                sb.Append(" already exists..");
                return RedirectToAction(nameof(this.Error), new { message = sb.ToString(), actionName = nameof(this.SignUp) });
            }
            if (string.IsNullOrEmpty(password))
            {
                StringBuilder sb = new StringBuilder("Account creating failed!! Reason: Password is empty..");
                return RedirectToAction(nameof(this.Error), new { message = sb.ToString(), actionName = nameof(this.SignUp) });
            }
            if (password != passwordCopy)
            {
                StringBuilder sb = new StringBuilder(
                    "Account creating failed!! Reason: Password confirmation does not match to original..");
                return RedirectToAction(nameof(this.Error), new { message = sb.ToString(), actionName = nameof(this.SignUp) });
            }

            Person person = new Person { UserName = userName, Password = password };
            try
            {
                _dbContext.People.Add(person);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Account creating failed!! Reason: ");
                string msg = ex.InnerException.Message != null ? ex.InnerException.Message : ex.Message;
                sb.Append(msg);
                return RedirectToAction(nameof(this.Error), new { message = sb.ToString(), actionName = nameof(this.SignUp) });
            }

            return RedirectToAction(nameof(this.SignUpConfirmation), new { userName = userName });
        }

        [HttpGet("SignUpConfirmation")]
        public IActionResult SignUpConfirmation(string userName)
        {
            ViewBag.UserName = userName;
            return View();
        }

        [HttpGet("SignOut")]
        public IActionResult SignOut()
        {
            if (Request.Cookies.ContainsKey(CookyUserName))
            {
                Response.Cookies.Delete(CookyUserName);
            }

            return RedirectToAction(nameof(this.SignIn));
        }

        [HttpGet("Remove")]
        public IActionResult Remove() { return View(); }

        [HttpGet("AccountRemoved")]
        public IActionResult AccountRemoved()
        {
            string userName = Request.Cookies[CookyUserName];

            var todoList = _dbContext.TodoList.Where(t => t.UserName == userName).ToList();
            foreach (Todo todo in todoList)
            {
                _dbContext.TodoList.Remove(todo);
            }
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Error occurred while removing nodes in TodoList!! Reason: ");
                string msg = ex.InnerException.Message != null ? ex.InnerException.Message : ex.Message;
                sb.Append(msg);
                return RedirectToAction(nameof(this.Error), new { message = sb.ToString() });
            }

            Person person = _dbContext.People.FirstOrDefault(p => p.UserName == userName);
            try
            {
                _dbContext.People.Remove(person);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Account removing failed!! Reason: ");
                string msg = ex.InnerException.Message != null ? ex.InnerException.Message : ex.Message;
                sb.Append(msg);
                return RedirectToAction(nameof(this.Error), new { message = sb.ToString() });
            }

            ViewBag.UserName = userName;
            return View();
        }
        [HttpGet("List")]
        public IActionResult List()
        {
            string userName = Request.Cookies[CookyUserName];
            var person = _dbContext.People.FirstOrDefault(p => p.UserName == userName);

            var todoList = _dbContext.TodoList.Where(t => t.UserName == person.UserName).OrderBy(t => t.DeadLine).ToList();

            var todoPair = new KeyValuePair<List<Todo>, List<Todo>>(new List<Todo>(), new List<Todo>());
            foreach (var todo in todoList)
            {
                if (todo.ActualDate == default(DateTime))
                {
                    todoPair.Key.Add(todo);
                }
                else
                {
                    todoPair.Value.Add(todo);
                }
            }

            ViewBag.UserName = userName;
            return View(todoPair);
        }

        [HttpPost("Add")]
        public IActionResult Add([FromForm]Todo todo)
        {
            if (todo.What == null)
            {
                return RedirectToAction(nameof(this.List));
            }
            todo.UserName = Request.Cookies[CookyUserName];

            try
            {
                _dbContext.TodoList.Add(todo);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Adding new node \"");
                sb.Append(todo.What);
                sb.Append("\" failed!! ");
                sb.Append("Reason: ");
                string msg = ex.InnerException.Message != null ? ex.InnerException.Message : ex.Message;
                sb.Append(msg);
                return RedirectToAction(nameof(this.Error), new { message = sb.ToString() });
            }

            return RedirectToAction(nameof(this.List));
        }

        [HttpGet("Complete")]
        public IActionResult Complete(Todo todo)
        {
            Todo finishedTodo = _dbContext.TodoList.FirstOrDefault(t =>
                t.UserName == todo.UserName &&
                t.What == todo.What);
            if (finishedTodo == null)
            {
                StringBuilder sb = new StringBuilder("Finishing node failed for user ");
                sb.Append(todo.UserName);
                sb.Append("!! Reason: Todo node not founded..");
                return RedirectToAction(nameof(this.Error), new { message = sb.ToString() });
            }

            finishedTodo.ActualDate = DateTime.Now;
            _dbContext.SaveChanges();

            return RedirectToAction(nameof(this.List));
        }

        [HttpGet("Clear")]
        public IActionResult Clear(Todo todo)
        {
            Todo removedTodo = _dbContext.TodoList.FirstOrDefault(t =>
                t.UserName == todo.UserName &&
                t.What == todo.What);
            if (removedTodo == null)
            {
                StringBuilder sb = new StringBuilder("Removing node failed for user ");
                sb.Append(todo.UserName);
                sb.Append("!! Reason: Todo node not founded..");
                return RedirectToAction(nameof(this.Error), new { message = sb.ToString() });
            }

            try
            {
                _dbContext.TodoList.Remove(removedTodo);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Removing node \"");
                sb.Append(todo.What);
                sb.Append("\" failed!! ");
                sb.Append("Reason: ");
                string msg = ex.InnerException.Message != null ? ex.InnerException.Message : ex.Message;
                sb.Append(msg);
                return RedirectToAction(nameof(this.Error), new { message = sb.ToString() });
            }

            return RedirectToAction(nameof(this.List));
        }

        [HttpGet("Error")]
        public IActionResult Error(string message = null, string actionName = nameof(this.List))
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Message = message,
                ActionName = actionName
            });
        }
    }
}
