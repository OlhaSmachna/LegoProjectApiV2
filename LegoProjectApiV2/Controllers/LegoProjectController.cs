using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LegoProjectApi.Controllers
{
    public abstract class LegoProjectController : ControllerBase
    {
        protected string EmailFromClaims()
        {
            ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
            return identity.Claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault().Value;
        }
    }
}
