using Microsoft.AspNetCore.Mvc;

namespace P013EStore.MVCUI.Areas.Admin.Controllers
{
    public class MainController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
