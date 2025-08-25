using Microsoft.AspNetCore.Mvc;

namespace WebAppSmartHYS.Controllers
{
    public class TablesController : Controller
    {
        public IActionResult Basic() => View();
        public IActionResult GenerateStyle() => View();
    }
}
