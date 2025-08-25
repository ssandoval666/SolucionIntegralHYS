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
using Newtonsoft.Json;

namespace WebAppSmartHYS.Areas.Identity.Pages.Account
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
			if (!string.IsNullOrEmpty(ErrorMessage))
			{
				ModelState.AddModelError(string.Empty, ErrorMessage);
			}

			returnUrl = returnUrl ?? Url.Content("~/");

			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

			ReturnUrl = returnUrl;
		}

		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
		{
			returnUrl = returnUrl ?? Url.Content("~/");

			if (ModelState.IsValid)
			{
				// This doesn't count login failures towards account lockout
				// To enable password failures to trigger account lockout, set lockoutOnFailure: true

				//var result = await _signInManager.PasswordSignInAsync("drlantern@gotbootstrap.com", "Password123!", false, lockoutOnFailure: false);
				await HttpContext.SignOutAsync();

				var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, true, lockoutOnFailure: false);

				if (result.Succeeded)
				{
					HttpContext.Session.Remove("oUser");
					HttpContext.Session.Remove("oUserRoles");
					var curUser = _signInManager.UserManager.Users.Where(x => x.UserName == Input.UserName).FirstOrDefault();
					var roles = _signInManager.UserManager.GetRolesAsync(curUser);
					
					HttpContext.Session.SetString("oUser", JsonConvert.SerializeObject(curUser));
					HttpContext.Session.SetString("oUserRoles", JsonConvert.SerializeObject(roles.Result));
					await _signInManager.RefreshSignInAsync(curUser);

					_logger.LogInformation("User logged in.");
					return LocalRedirect(returnUrl);

				}
				else
				{
					ModelState.AddModelError(string.Empty, "Error de credenciales");
					return Page();
				}






			}

			// If we got this far, something failed, redisplay form
			return Page();
		}
	}
}
