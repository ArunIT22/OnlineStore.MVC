using Microsoft.AspNetCore.Mvc;

namespace OnlineStore.MVC.Controllers
{
    public class AjaxProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
    }
}
