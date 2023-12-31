using LMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers
{
    public class DashboardController : Controller
    {
        private Repository repo;
        public DashboardController()
        {
            repo = new Repository();
            // repo.I();
        }
        
        private bool isValidBook(Book book)
        {
            int number;
            return !book.Name.Any(char.IsDigit) && !book.Author.Any(char.IsDigit) && int.TryParse(book.Iban, out number);
        }

        [HttpGet]
        public IActionResult index()
        {
            if (!HttpContext.Request.Cookies.ContainsKey("adminId"))
            {
                return RedirectToAction("Index", "login");
            }
            List<Book> listOfBooks = repo.GetBooks();
            return View(listOfBooks);
        }

        // Delete a book from table
        [HttpPost]
        public IActionResult index(string Iban)
        {
            if (!HttpContext.Request.Cookies.ContainsKey("adminId"))
            {
                return RedirectToAction("Index", "login");
            }
            repo.DeleteBook(Iban);
            return index();
        }

        [HttpGet]
        public IActionResult AddBook()
        {
            if (!HttpContext.Request.Cookies.ContainsKey("adminId"))
            {
                return RedirectToAction("Index", "login");
            }
            return View();
        }


        [HttpPost]
        public IActionResult AddBook(Book book)
        {
            if (!HttpContext.Request.Cookies.ContainsKey("adminId"))
            {
                return RedirectToAction("Index", "login");
            }
            if (ModelState.IsValid)
            {
                if (repo.isBookAlreadyExist(book.Iban))
                {
                    ModelState.AddModelError("", $"IBAN {book.Iban} already exists");
                }
                else if (isValidBook(book))
                {
                    repo.AddBook(book);
                    ModelState.AddModelError("", "Book Added");
                }
                else
                    ModelState.AddModelError("", "Invalid data");
            }
            return AddBook();
        }

        [HttpGet]
        public IActionResult EditBook()
        {
            if (!HttpContext.Request.Cookies.ContainsKey("adminId"))
            {
                return RedirectToAction("Index", "login");
            }
            return View(ViewBag.book);
        }


        [HttpPost]
        public IActionResult EditBook(Book book)
        {
            if (!HttpContext.Request.Cookies.ContainsKey("adminId"))
            {
                return RedirectToAction("Index", "login");
            }
            ViewBag.book = book;
            if (ModelState.IsValid)
            {
                if (isValidBook(book))
                {
                    repo.editBook(book);
                    return EditBook();
                }
                else
                    ModelState.AddModelError("", "Invalid data");
            }
            return EditBook();
        }

        [HttpGet]
        public IActionResult ApproveRequests()
        {
            if (!HttpContext.Request.Cookies.ContainsKey("adminId"))
            {
                return RedirectToAction("Index", "login");
            }
            List<RequestBook> requestList = repo.GetRequests();
            return View(requestList);
        }

        [HttpPost]
        public IActionResult ApproveRequests(RequestBook request)
        {
            if (!HttpContext.Request.Cookies.ContainsKey("adminId"))
            {
                return RedirectToAction("Index", "login");
            }
            repo.approveRequest(request);
            return ApproveRequests();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            if (!HttpContext.Request.Cookies.ContainsKey("adminId"))
            {
                return RedirectToAction("Index", "login");
            }
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(Password password)
        {
            if (!HttpContext.Request.Cookies.ContainsKey("adminId"))
            {
                return RedirectToAction("Index", "login");
            }
            if (password.NewPassword == password.ConfirmPassword)
            {
                string userId = HttpContext.Request.Cookies["userId"];
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
        public IActionResult ViewUserPassword()
        {
            if (!HttpContext.Request.Cookies.ContainsKey("adminId"))
            {
                return RedirectToAction("Index", "login");
            }
            return View();
        }

        [HttpPost]
        public IActionResult ViewUserPassword(string userId)
        {
            if (!HttpContext.Request.Cookies.ContainsKey("adminId"))
            {
                return RedirectToAction("Index", "login");
            }
            string password = repo.GetUserPassword(userId);
            if (password != "")
            {
                ModelState.AddModelError("", $"Password: {password}");
            }
            else
            {
                ModelState.AddModelError("", $"User Id: {userId} not found");
            }
            return ViewUserPassword();
        }

        public IActionResult IssueBooks()
        {
            if (!HttpContext.Request.Cookies.ContainsKey("adminId"))
            {
                return RedirectToAction("Index", "login");
            }
            List<IssuedBook> issuedBooks = repo.GetIssuedBooks();
            return View(issuedBooks);
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            if (!HttpContext.Request.Cookies.ContainsKey("adminId"))
            {
                return RedirectToAction("Index", "login");
            }
            return View();
        }

        [HttpPost]
        public IActionResult AddUser(User newUser)
        {
            if (!HttpContext.Request.Cookies.ContainsKey("adminId"))
            {
                return RedirectToAction("Index", "login");
            }
            bool isAdded = repo.AddUser(newUser);
            if (!isAdded)
            {
                ModelState.AddModelError("", $"User Id: {newUser.Id} is already taken");
            }
            else
            {
                ModelState.AddModelError("", $"User added");
            }
            return View();
        }

        [HttpGet]
        public IActionResult ViewUsers()
        {
            if (!HttpContext.Request.Cookies.ContainsKey("adminId"))
            {
                return RedirectToAction("Index", "login");
            }
            List<User> users = repo.GetUsers();
            return View(users);
        }

        // Delete a user
        [HttpPost]
        public IActionResult ViewUsers(string Id)
        {
            if (!HttpContext.Request.Cookies.ContainsKey("adminId"))
            {
                return RedirectToAction("Index", "login");
            }
            bool isDeleted = repo.DeleteUser(Id);
            if (!isDeleted)
            {
                ModelState.AddModelError("", "This user has Issued some books");
            }
            return ViewUsers();
        }

        [HttpGet]
        public IActionResult Notification()
        {
            if (HttpContext.Request.Cookies.ContainsKey("adminId"))
            {
                Response.Cookies.Delete("userId");
            }
            return View();
        } 

        [HttpGet]
        public IActionResult Logout()
        {
            if (HttpContext.Request.Cookies.ContainsKey("adminId"))
            {
                Response.Cookies.Delete("userId");
            }
            return RedirectToAction("index", "login");
        }

    }
}
