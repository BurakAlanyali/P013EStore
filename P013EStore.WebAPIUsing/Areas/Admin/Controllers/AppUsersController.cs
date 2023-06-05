using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;


namespace P013EStore.WebAPIUsing.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AppUsersController : Controller
    {
        private readonly HttpClient _httpClient;// _httpClient nesnesini kullanarak api lere istek gönderebiliriz.
        private readonly string _apiAdres= "https://localhost:7115/api/AppUsers";//api adresini web api projesini çalıştırdığımızda çıkan return URL kısmından alabiliriz. web api projesinde properties altındaki launchsettings.json'dan çekebiliriz.
        public AppUsersController(HttpClient httpClient)
        {
            _httpClient = httpClient; // bu httpClient'ın çalışması için WebAPI'nin de aynı anda çalışıyor olması gerekiyor.
            //Aynı anda 2 proje çalıştırabilmek için Solution'a sağ tıklayıp açılan menüden configure startup projects deyip açılan pencereden aynı anda çalışmasını istediğimiz projeleri seçiyoruz.
        }

        // GET: AppUsersController
        public async Task<ActionResult> Index()
        {
            var model = await _httpClient.GetFromJsonAsync<List<AppUser>>(_apiAdres);// _httpClient nesnesi içindeki getformjsonasync metodu kendisine verdiğimiz _apiAdres deki url e get isteği gönderir ve oradan gelen json formatındaki app user listesini
            return View(model);
        }

        // GET: AppUsersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AppUsersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AppUsersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AppUser collection)
        {
            try
            {
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

        // GET: AppUsersController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var model = await _httpClient.GetFromJsonAsync<AppUser>(_apiAdres + "/"+ id);
            return View(model);
        }

        // POST: AppUsersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, AppUser collection)
        {
            try
            {
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
            return View();
        }

        // GET: AppUsersController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var model = await _httpClient.GetFromJsonAsync<AppUser>(_apiAdres + "/" + id);
            return View(model);
        }

        // POST: AppUsersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, AppUser collection)
        {
            try
            {
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
