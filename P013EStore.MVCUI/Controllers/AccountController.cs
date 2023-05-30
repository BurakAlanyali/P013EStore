using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.MVCUI.Models;
using P013EStore.Service.Abstract;

namespace P013EStore.MVCUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IService<AppUser> _appUserService;

        public AccountController(IService<AppUser> appUserService)
        {
            _appUserService = appUserService;
        }

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
                var kullanici =await _appUserService.GetAsync(u=>u.Id==kullaniciID);
                return View(kullanici);
            }
            
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUser(AppUser appUser)
        {
            try
            {
                var kullaniciID = HttpContext.Session.GetInt32("userId");
                var kullanici = await _appUserService.GetAsync(u => u.Id == kullaniciID);
                if (kullanici is not null)
                {
                    kullanici.Name = appUser.Name;
                    kullanici.Email = appUser.Email;
                    kullanici.Password = appUser.Password;
                    kullanici.Surname = appUser.Surname;
                    kullanici.Phone = appUser.Phone;
                    _appUserService.Update(kullanici);
                    await _appUserService.SaveAsync();
                    return RedirectToAction(nameof(Index));
                }
                
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Hata Oluştu!");
                
            }
            return View("Index",appUser);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel user)
        {
            var kullanici= await _appUserService.GetAsync(x=>x.Email==user.Email && x.Password==user.Password && x.IsActive);
            if(kullanici is null)
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
            
            return RedirectToAction("Index","Home");
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (_appUserService.Get(x=>x.Email==appUser.Email) is not null)
                    {
                        ModelState.AddModelError("", "Bu Email kullanılmaktadır... Onunla yeni kayıt oluşturamazsınız!");
                        return View();
                    }
                    appUser.UserGuid = Guid.NewGuid();
                    appUser.IsActive = true;
                    appUser.IsAdmin = false;
                    await _appUserService.AddAsync(appUser);
                    await _appUserService.SaveAsync();
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
