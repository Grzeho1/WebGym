using Microsoft.AspNetCore.Mvc;

namespace WebGym.Controllers
{
    public class PanelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
