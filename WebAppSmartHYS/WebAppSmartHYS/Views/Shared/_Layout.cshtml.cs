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

namespace WebAppSmartHYS.Views.Shared
{
	[AllowAnonymous]
	public class LayoutModel : PageModel
	{
		public async Task OnGetAsync(string returnUrl = null)
		{
		}
	}
}
