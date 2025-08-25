using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppSmartDS.Areas.Authorization.Pages
{
    [Authorize]
    public class RoleModel : PageModel
    {
    }
}
