using Microsoft.AspNetCore.Mvc;

namespace WebAppShares.Controllers
{
    public abstract class AProductsController : Controller
    {
        public abstract Task<IActionResult> Index(string SearchText, int page);

        public abstract Task<IActionResult> Details(int? id);
    }
}
