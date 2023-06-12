using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using P013EStore.Core.Entities;
using P013EStore.WebAPIUsing.Models;

namespace P013EStore.WebAPIUsing.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private readonly string _apiAdres = "https://localhost:7115/api/AppUsers";
        public async Task<IActionResult> Index()
        {
            var kullaniciID = HttpContext.Session.GetInt32("userId");
            if (kullaniciID is null)
            {
                TempData["Message"] = "<div class='alert alert-danger'>Lütfen Giriş Yapınız!!!</div>";
                return RedirectToAction(nameof(Login));
            }
            else
            {
                var kullanici = await _httpClient.GetFromJsonAsync<AppUser>(_apiAdres + "/" + kullaniciID);
                return View(kullanici);
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUser(AppUser appUser)
        {
            try
            {
                var kullaniciID = HttpContext.Session.GetInt32("userId");
                var kullanici = await _httpClient.GetFromJsonAsync<AppUser>(_apiAdres + "/" + kullaniciID);
                if (kullanici is not null)
                {
                    kullanici.Name = appUser.Name;
                    kullanici.Email = appUser.Email;
                    kullanici.Password = appUser.Password;
                    kullanici.Surname = appUser.Surname;
                    kullanici.Phone = appUser.Phone;
                    _httpClient.PutAsJsonAsync(_apiAdres,kullanici);
                    return RedirectToAction(nameof(Index));
                }

            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Hata Oluştu!");

            }
            return View("Index", appUser);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel user)
        {
            var kullaniciListesi = await _httpClient.GetFromJsonAsync<List<AppUser>>(_apiAdres);
            var kullanici = kullaniciListesi.Where(x => x.Email == user.Email && x.Password == user.Password && x.IsActive).FirstOrDefault();
            if (kullanici is null)
            {
                ModelState.AddModelError("", "Giriş Başarısız!");
            }
            else
            {
                HttpContext.Session.SetInt32("userId", kullanici.Id);
                HttpContext.Session.SetString("userGuid", kullanici.UserGuid.ToString());
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public IActionResult SignIn()
        {
            return View();
        }
        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.Remove("userId");
                HttpContext.Session.Remove("userGuid");
            }
            catch (Exception)
            {

                HttpContext.Session.Clear();
            }

            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var kullaniciListesi = await _httpClient.GetFromJsonAsync<List<AppUser>>(_apiAdres);
                    var kullanici = kullaniciListesi.Where(x => x.Email == appUser.Email).FirstOrDefault();
                    if (kullanici is not null)
                    {
                        ModelState.AddModelError("", "Bu Email kullanılmaktadır... Onunla yeni kayıt oluşturamazsınız!");
                        return View();
                    }
                    appUser.UserGuid = Guid.NewGuid();
                    appUser.IsActive = true;
                    appUser.IsAdmin = false;
                    await _httpClient.PostAsJsonAsync(_apiAdres, appUser);
                    
                    TempData["Message"] = "<div class='alert alert-success'>Yeni Kayıt Başarıyla Oluşturuldu! Teşekkürler..</div>";
                }
                catch (Exception)
                {

                    ModelState.AddModelError("", "Hata Oluştu!");
                    return RedirectToAction("SignIn");
                }
            }
            return View();
        }
        public IActionResult NewPassword()
        {
            return View();
        }
    }
}
