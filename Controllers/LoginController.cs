using LMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(User user)
        {
            if (ModelState.IsValid)
            {
                Repository repo = new Repository();
                User? result = repo.getUser(user);
                if (result != null && result.Admin == true)
                {
                    if (!HttpContext.Request.Cookies.ContainsKey("userId"))
                    {
                        HttpContext.Response.Cookies.Append("adminId", user.Id);
                    }
                    return RedirectToAction("index", "Dashboard");
                }
                else if (result != null && result.Admin == false)
                {
                    if (!HttpContext.Request.Cookies.ContainsKey("userId"))
                    {
                        HttpContext.Response.Cookies.Append("userId", user.Id);
                    }
                    return RedirectToAction("index", "UserDashboard");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password");
                }
            }
            return View();
        }
    }
}