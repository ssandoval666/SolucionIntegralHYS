using WebAppSmartHYS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace WebAppSmartHYS.Controllers
{
    [AllowAnonymous]
    public class CapacitacionesCirculacion : Controller
        
    {
        /*
        private readonly ILogger<CapacitacionesController> _logger;

        public CapacitacionesController(ILogger<CapacitacionesController> logger)
        {
            _logger = logger;
        }
        */

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
