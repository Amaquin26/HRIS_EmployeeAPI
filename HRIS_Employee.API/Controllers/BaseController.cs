using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HRIS_Employee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected string? GetUserOID()
        {
            var userClaimsPrincipal = HttpContext.User;

            var entraObjectId = User.FindFirstValue("http://schemas.microsoft.com/identity/claims/objectidentifier");

            return entraObjectId;
        }
    }
}
