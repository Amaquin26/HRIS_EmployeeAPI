using HRIS_Employee.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRIS_Employee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IdentityController(IAuthUserDetailsService authUserDetailsService) : BaseController
    {
        [HttpGet("me")]
        public async Task<IActionResult> GetMyUserDetails()
        {
            var entraObjectId = GetUserOID();

            if (string.IsNullOrEmpty(entraObjectId))
                return Unauthorized("OID claim not found.");

            var result = await authUserDetailsService.GetMyUserDetails(entraObjectId);

            if (result == null)
                return StatusCode(403, new ProblemDetails()
                {
                    Status = StatusCodes.Status403Forbidden,
                    Title = "No account found",
                    Detail = "You are registered in the active directory, but does not have an account to use this app yet. Contact HR or help desk if this is a mistake."
                });

            return Ok(result);
        }
    }
}
