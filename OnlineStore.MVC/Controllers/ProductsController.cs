using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol;
using OnlineStore.MVC.Models;

namespace OnlineStore.MVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string baseUrl = string.Empty;

        public ProductsController(IConfiguration configuration)
        {
            _configuration = configuration;
            baseUrl = _configuration.GetValue<string>("apiUrl");
        }

        public async Task<IActionResult> Index()
        {
            using (HttpClient client = new())
            {
                var url = string.Concat(baseUrl, "Products");
                var response = await client.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync(); //Json Serialization
                    var product = JsonConvert.DeserializeObject<List<ProductViewModel>>(result);
                    return View(product);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ModelState.AddModelError("", "Unauthorized Access. Please provide valid credentials");
                    return View();
                }

                ModelState.AddModelError("", "Unable to process your request. Server Error");
                return View();
            }
        }

        [NonAction]
        public async Task<IEnumerable<Category>?> GetCategories()
        {
            using (HttpClient client = new())
            {
                var url = string.Concat(baseUrl, "Products/CategoryList");
                var response = await client.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync(); //Json Serialization
                    var categories = JsonConvert.DeserializeObject<List<Category>>(result);
                    return categories;
                }
                return null;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (HttpContext.Session.GetString("token") == null)
            {
                return RedirectToAction("Login", "Accounts");
            }
            ViewBag.Categories = await GetCategories();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddOrUpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Categories = await GetCategories();
                using (HttpClient client = new())
                {
                    var url = string.Concat(baseUrl, "Products");
                    //Add Token to AuthorizationHeader
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", HttpContext.Session.GetString("token"));
                    var response = await client.PostAsJsonAsync(url, model);
                    if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("Index");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        ModelState.AddModelError("", "Invalid Request");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        ModelState.AddModelError("", "Unauthorized Access. Please provide valid credentials");
                        return View();
                    }

                    ModelState.AddModelError("", "Unable to process your request. Server Error");
                    return View();
                }
            }
            
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Categories = await GetCategories();
            using (HttpClient client = new())
            {
                var url = string.Concat(baseUrl, $"Products/{id}");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", HttpContext.Session.GetString("token"));
                var response = await client.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var product = JsonConvert.DeserializeObject<AddOrUpdateViewModel>(result);
                    return View(product);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ModelState.AddModelError("", "No Product available");
                    return View(new AddOrUpdateViewModel());
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ModelState.AddModelError("", "Unauthorized Access. Please provide valid credentials");
                    return View();
                }

                ModelState.AddModelError("", "Unable to process your request. Server Error");
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(AddOrUpdateViewModel model)
        {
            using (HttpClient client = new())
            {
                var url = string.Concat(baseUrl, "Products");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", HttpContext.Session.GetString("token"));
                var response = await client.PutAsJsonAsync(url, model);
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return RedirectToAction("Index");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ModelState.AddModelError("", "Unauthorized Access. Please provide valid credentials");
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", "Unable to process your request. Server Error");
                    ViewBag.Categories = await GetCategories();
                    return View();
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            ViewBag.Categories = await GetCategories();
            using (HttpClient client = new())
            {
                var url = string.Concat(baseUrl, $"Products/{id}");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", HttpContext.Session.GetString("token"));
                var response = await client.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var product = JsonConvert.DeserializeObject<ProductViewModel>(result);
                    return View(product);
                }
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ModelState.AddModelError("", "No Product available");
                    return View(new ProductViewModel());
                }
                ViewData["ErrorMessage"] = "Unable to process your request. Server Error";
                return View();
            }
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (HttpClient client = new())
            {
                var url = string.Concat(baseUrl, $"Products?id={id}");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", HttpContext.Session.GetString("token"));
                var response = await client.DeleteAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ModelState.AddModelError("", "No Product available");
                    return View("Delete", new ProductViewModel());
                }
                ViewData["ErrorMessage"] = "Unable to process your request. Server Error";
                return View();
            }
        }
    }
}
