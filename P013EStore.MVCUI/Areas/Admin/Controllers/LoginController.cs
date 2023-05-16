using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.MVCUI.Models;
using P013EStore.Service.Abstract;
using System.Security.Claims;

namespace P013EStore.MVCUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly IService<AppUser> _service;

        public LoginController(IService<AppUser> service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(AdminLoginViewModel admin)
        {
            try
            {
                var kullanici = await _service.GetAsync(k=>k.IsActive && k.Email==admin.Email && k.Password==admin.Password);
                if (kullanici != null)
                {
                    var kullaniciYetkileri = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email,kullanici.Email),
                        new Claim("Role",kullanici.IsAdmin ? "Admin" : "User"),
                        new Claim("UserGuid",kullanici.UserGuid.ToString()),
                    };
                    var kullanicikimligi = new ClaimsIdentity(kullaniciYetkileri, "Login");
                    ClaimsPrincipal claimsPrincipal = new(kullanicikimligi);
                    await HttpContext.SignInAsync(claimsPrincipal); 
                    return Redirect("/Admin/Main");
                }

            }
            catch (Exception hata)
            {
                ModelState.AddModelError("", "Hata Oluştu!" + hata.Message);

            }
            return View();
        }
        [Route("/Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();// Sistemden çıkış yap
            return Redirect("/Admin/Login");// tekrardan giriş sayfasına yönlendirme
        }
        [Route("/AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
