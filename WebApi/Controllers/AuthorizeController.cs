using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AnaDron.SDTest.WebApi.Controllers {
	[Authorize]
	public abstract class AuthorizeController:ApiController {
		protected int UserId => int.Parse(User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value);
	}
}
