using LMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers
{
    public class UserDashboardController : Controller
    {
        private Repository repo;
        public UserDashboardController()
        {
            repo = new Repository();
        }

        [HttpGet]
        public IActionResult index()
        {
             if (!HttpContext.Request.Cookies.ContainsKey("userId"))
            {
                return RedirectToAction("Index", "login");
            }
            List<Book> list = repo.GetBooks();
            return View(list);
        }

        [HttpPost]
        public IActionResult index(string Iban)
        {
             if (!HttpContext.Request.Cookies.ContainsKey("userId"))
            {
                return RedirectToAction("Index", "login");
            }
            string userId = HttpContext.Request.Cookies["userId"];
            int info = repo.AddBook(userId, Iban);
            if (info == 1)
            {
                ModelState.AddModelError("", "You have request 1 book and 1 Issued");
            }
            else if (info == 2)
            {
                ModelState.AddModelError("", "You have already issued 2 books");
            }
            else if (info == 3)
            {
                ModelState.AddModelError("", "You have already request 2 books");
            }
            return index();
        }

        [HttpGet]
        public IActionResult IssuedBooks()
        {
             if (!HttpContext.Request.Cookies.ContainsKey("userId"))
            {
                return RedirectToAction("Index", "login");
            }
            string userId = HttpContext.Request.Cookies["userId"];
            List<Book> list = repo.GetIssuedBooks(userId);
            System.Console.WriteLine(userId);
            return View(list);
        }

        [HttpPost]
        public IActionResult IssuedBooks(string iban)
        {
            if (!HttpContext.Request.Cookies.ContainsKey("userId"))
            {
                return RedirectToAction("Index", "login");
            }
            string userId = HttpContext.Request.Cookies["userId"];
            repo.returnBook(userId, iban);
            return IssuedBooks();
        }
   
        [HttpGet]
        public IActionResult ChangePassword()
        {
            if (!HttpContext.Request.Cookies.ContainsKey("userId"))
            {
                return RedirectToAction("Index", "login");
            }
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(Password password)
        {
            if (!HttpContext.Request.Cookies.ContainsKey("userId"))
            {
                return RedirectToAction("Index", "login");
            }
            string userId = HttpContext.Request.Cookies["userId"];
            if (password.NewPassword == password.ConfirmPassword)
            {
                bool isChange = repo.ChangePassword(password, userId);
                if (!isChange)
                {
                    ModelState.AddModelError("", "Current password is incorrect");
                }
                else
                {
                    ModelState.AddModelError("", "Password Changed!");
                }
            }
            else
            {
                ModelState.AddModelError("", "New and Confirm Password do not match");
            }
            return ChangePassword();
        }


        [HttpGet]
        public IActionResult Logout()
        {
            if (HttpContext.Request.Cookies.ContainsKey("userId"))
            {
                Response.Cookies.Delete("userId");
            }
            return RedirectToAction("Index", "Login");
        }
    }
}
