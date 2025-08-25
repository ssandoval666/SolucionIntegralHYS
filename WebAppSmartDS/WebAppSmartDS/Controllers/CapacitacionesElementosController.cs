using WebAppSmartDS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace WebAppSmartDS.Controllers
{
    public class CapacitacionesElementosController : Controller
        
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
