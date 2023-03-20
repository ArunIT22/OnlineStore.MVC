using Microsoft.AspNetCore.Mvc;
using OnlineStore.MVC.Models;

namespace OnlineStore.MVC.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IConfiguration _configuration;

        public AccountsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            using (var client = new HttpClient())
            {
                string baseUrl = _configuration.GetValue<string>("apiUrl");
                var url = string.Concat(baseUrl, "Accounts/Register");
                var result = await client.PostAsJsonAsync(url, register);
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Server Error");
            }
            return View(register);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                using(var client = new HttpClient())
                {
                    string baseUrl = _configuration.GetValue<string>("apiUrl");
                    var url = string.Concat(baseUrl, "Accounts/Login");
                    var result = await client.PostAsJsonAsync(url, loginVM);
                    if(result.IsSuccessStatusCode)
                    {
                        string token = await result.Content.ReadAsStringAsync();
                        HttpContext.Session.SetString("token", token);
                        return RedirectToAction("Index", "Home");
                    }
                    else if(result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        ModelState.AddModelError("", "Invalid username or password");
                    }
                }
            }
            return View(loginVM);
        }
    }
}
