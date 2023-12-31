using Microsoft.AspNetCore.Mvc;

namespace LMS.Component 
{
    public class BookList : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}