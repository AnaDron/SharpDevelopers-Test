using AnaDron.SDTest.Model;
using Microsoft.AspNetCore.Mvc;

namespace AnaDron.SDTest.WebApi.Controllers {
	[Route("api")]
	public sealed class HomeController:AuthorizeController {
		readonly IUserService _userService;

		public HomeController(IUserService userService) {
			_userService = userService;
		}

		[HttpGet]
		public ActionResult UserInfo() {
			UserModel user;
			try {
				user = _userService.Get(UserId);
			} catch (UserNotFoundException) {
				return Unauthorized();
			}

			return Ok(new { name = user.Name, balance = user.Balance });
		}
	}
}
