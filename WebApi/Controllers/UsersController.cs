using System.Collections.Generic;
using System.Linq;
using AnaDron.SDTest.Model;
using AnaDron.SDTest.WebApi.Requests;
using Microsoft.AspNetCore.Mvc;

namespace AnaDron.SDTest.WebApi.Controllers {
	[Route("api/users")]
	public sealed class UsersController:AuthorizeController {
		readonly IUserService _userService;
		readonly ITransactionService _transactionService;

		public UsersController(IUserService userService, ITransactionService transactionService) {
			_userService = userService;
			_transactionService = transactionService;
		}

		[HttpGet]
		public ActionResult GetOther([FromQuery] UsersGetOtherRequest request) {
			var limit = request.Limit ?? 5;
			List<UserModel> users;
			try {
				users = _userService.GetOther(UserId, request.Template, limit).ToList();
			} catch (UserNotFoundException) {
				return Unauthorized();
			}

			return Ok(new { users = users.Select(x => new { Id = x.Id, Name = x.Name }) });
		}
	}
}
