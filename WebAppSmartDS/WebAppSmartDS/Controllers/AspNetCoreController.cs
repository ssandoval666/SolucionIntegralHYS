using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAppSmartDS.Controllers
{
    [AllowAnonymous]
    public class AspNetCoreController : Controller
    {
        public IActionResult Welcome() => View();
        public IActionResult Interactive() => View();
        public IActionResult Editions() => View();
        public IActionResult Faq() => View();
    }
}
