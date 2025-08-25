using WebAppSmartHYS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Linq;

namespace WebAppSmartHYS.Controllers
{
    [AllowAnonymous]
    public class MainController : Controller

    {

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public MainController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            ValidarUsuarioGuest();

            return View();
        }

        private async void ValidarUsuarioGuest()
        {

            await HttpContext.SignOutAsync();

            var oUser = HttpContext.Session.GetString("oUser") == null ? null : JsonConvert.DeserializeObject<IdentityUser>(HttpContext.Session.GetString("oUser"));


            if (oUser == null)
            {

                var result = await _signInManager.PasswordSignInAsync("GUEST", "Password123!", true, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var curUser = _signInManager.UserManager.Users.Where(x => x.UserName == "GUEST").FirstOrDefault();
                    var roles = _signInManager.UserManager.GetRolesAsync(curUser);

                    //var curUser = await _signInManager.UserManager.GetUserAsync(HttpContext.User);
                    HttpContext.Session.SetString("oUser", JsonConvert.SerializeObject(curUser));
                    HttpContext.Session.SetString("oUserRoles", JsonConvert.SerializeObject(roles.Result));

                    await _signInManager.RefreshSignInAsync(curUser);
                }
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
