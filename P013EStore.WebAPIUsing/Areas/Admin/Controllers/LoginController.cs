using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using P013EStore.Core.Entities;
using P013EStore.WebAPIUsing.Models;
using System.Security.Claims;

namespace P013EStore.WebAPIUsing.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiAdres = "https://localhost:7115/api/AppUsers";
        public LoginController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IActionResult Index(string ReturnUrl)
        {
            var model = new AdminLoginViewModel();
            model.ReturnUrl = ReturnUrl;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> IndexAsync(AdminLoginViewModel admin)
        {
            try
            {
                var userList = await _httpClient.GetFromJsonAsync<List<AppUser>>(_apiAdres);
                var account = userList.FirstOrDefault(x=>x.Email == admin.Email && x.Password == admin.Password && x.IsActive);
                if (account == null)
                {
                    ModelState.AddModelError("", "Giriş Başarısız!");
                }
                else
                {
                    var kullaniciYetkileri = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email,account.Email),
                        new Claim("Role",account.IsAdmin ? "Admin" : "User"),
                        new Claim("UserGuid",account.UserGuid.ToString()),
                    };
                    var kullanicikimligi = new ClaimsIdentity(kullaniciYetkileri, "Login");
                    ClaimsPrincipal claimsPrincipal = new(kullanicikimligi);
                    await HttpContext.SignInAsync(claimsPrincipal);
                    return Redirect(string.IsNullOrEmpty(admin.ReturnUrl) ? "/Admin/Main" : admin.ReturnUrl);
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Hata Oluştu!");
                
            }
            return View(admin);
        }
    }
}
