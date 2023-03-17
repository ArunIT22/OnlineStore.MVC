using Microsoft.AspNetCore.Mvc;
using OnlineStore.MVC.Models;

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

        //[HttpPost, ActionName("Create")]
        //public IActionResult CreateSuccessful()
        //{
        //    return View();
        //}
    }
}
