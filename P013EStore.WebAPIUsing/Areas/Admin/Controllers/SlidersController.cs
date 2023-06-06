using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using P013EStore.Core.Entities;
using P013EStore.WebAPIUsing.Utils;

namespace P013EStore.WebAPIUsing.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlidersController : Controller
    {
        private readonly HttpClient _httpClient;

        public SlidersController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        private readonly string _apiAdres = "https://localhost:7115/api/Sliders";
        // GET: SlidersController
        public async Task<ActionResult> Index()
        {
            var model = await _httpClient.GetFromJsonAsync<List<Slider>>(_apiAdres);
            return View(model);
        }

        // GET: SlidersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SlidersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SlidersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Slider collection, IFormFile? Image)
        {
            try
            {
                if (Image is not null)
                {
                    collection.Image = await FileHelper.FileLoaderAsync(Image);
                }
                var response = await _httpClient.PostAsJsonAsync(_apiAdres, collection);
                if (response.IsSuccessStatusCode) // api'den gelen istek başarılı ise buraya girer
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            catch
            {

                ModelState.AddModelError("", "Hata Oluştu!");
            }
            
            return View(collection);
        }

        // GET: SlidersController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            var model = await _httpClient.GetFromJsonAsync<Slider>(_apiAdres + "/" + id);
            return View(model);
        }

        // POST: SlidersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, Slider collection, IFormFile? Image, bool? resmiSil)
        {
            try
            {
                if (resmiSil is not null && resmiSil == true)
                {
                    FileHelper.FileRemover(collection.Image);
                    collection.Image = "";
                }
                if (Image is not null)
                {
                    collection.Image = await FileHelper.FileLoaderAsync(Image);
                }
                var response = await _httpClient.PutAsJsonAsync(_apiAdres, collection);
                if (response.IsSuccessStatusCode) // api'den gelen istek başarılı ise buraya girer
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                ModelState.AddModelError("", "Hata Oluştu!");
            }
            
            return View(collection);
        }

        // GET: SlidersController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var model = await _httpClient.GetFromJsonAsync<Slider>(_apiAdres + "/" + id);
            return View(model);
        }

        // POST: SlidersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, Slider collection)
        {
            try
            {
                FileHelper.FileRemover(collection.Image);
                await _httpClient.DeleteAsync(_apiAdres + "/" + id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
