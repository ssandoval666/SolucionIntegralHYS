using Microsoft.AspNetCore.Mvc;

namespace WebAppSmartDS.Controllers
{
    public class TablesController : Controller
    {
        public IActionResult Basic() => View();
        public IActionResult GenerateStyle() => View();
    }
}
