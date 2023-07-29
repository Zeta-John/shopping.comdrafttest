using Microsoft.AspNetCore.Mvc;

namespace shoppingcomdraft5.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
