using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppSmartHYS.Areas.Authorization.Pages
{
    [Authorize]
    public class UserModel : PageModel
    {
    }
}
