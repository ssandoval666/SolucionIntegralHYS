using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Diagnostics.Eventing.Reader;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using System;

namespace WebAppSmartDS.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
			await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();

			if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            using (var httpClient = new HttpClient())
            {
                var cliente = new WebApiDS.WebApi("https://localhost:7245", httpClient);
                

                if (ModelState.IsValid)
                {
                    
                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                    var oUsuarioRta = cliente.ValidarAsync(Input.UserName, Input.Password, "1").Result;

                    if (oUsuarioRta.UserName != null)                    {
                       

                        var result = await _signInManager.PasswordSignInAsync("drlantern@gotbootstrap.com", "Password123!", false, lockoutOnFailure: false);


                        HttpContext.Session.SetString("usuario", oUsuarioRta.Nombre + " " + oUsuarioRta.Apellido);
                        HttpContext.Session.SetString("username", oUsuarioRta.UserName);
                        HttpContext.Session.SetString("profile", oUsuarioRta.Foto);
                        _logger.LogInformation("User logged in.");
                        return LocalRedirect(returnUrl);
                        /*
                        if (result.Succeeded)
                        {
                            
                        }
                        if (result.RequiresTwoFactor)
                        {
                            return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                        }
                        if (result.IsLockedOut)
                        {
                            _logger.LogWarning("User account locked out.");
                            return RedirectToPage("./Lockout");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                            return Page();
                        }
                        */
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Error de credenciales");
                        return Page();
                    }

                }

            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
