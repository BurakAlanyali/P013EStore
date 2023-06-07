using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.WebAPIUsing.Models;
using System.Diagnostics;

namespace P013EStore.WebAPIUsing.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiAdres = "https://localhost:7115/api/";
        public HomeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _httpClient.GetFromJsonAsync<List<Product>>(_apiAdres + "Products");
            var model = new HomePageViewModel
            {
                Products = products,
                Sliders = await _httpClient.GetFromJsonAsync<List<Slider>>(_apiAdres + "Sliders")
        };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}