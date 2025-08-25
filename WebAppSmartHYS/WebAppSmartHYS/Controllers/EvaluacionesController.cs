using WebAppSmartHYS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace WebAppSmartHYS.Controllers
{
    [AllowAnonymous]
    public class EvaluacionesController : Controller
    {
        /*
        private readonly ILogger<EvaluacionesController> _logger;

        public EvaluacionesController(ILogger<EvaluacionesController> logger)
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
