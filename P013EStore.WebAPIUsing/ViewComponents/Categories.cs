using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;


namespace P013EStore.WebAPIUsing.ViewComponents
{
    public class Categories : ViewComponent
    {
        private readonly HttpClient _httpClient;

        public Categories(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private readonly string _apiAdres = "https://localhost:7115/api/Categories";

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _httpClient.GetFromJsonAsync<List<Category>>(_apiAdres));
        }
    }
}
