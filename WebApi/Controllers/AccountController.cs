using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AnaDron.SDTest.Model;
using AnaDron.SDTest.WebApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AnaDron.SDTest.WebApi.Controllers {
	[Route("api/auth")]
	[AllowAnonymous]
	public sealed class AccountController:ApiController {
		readonly IUnitOfWork _unitOfWork;
		readonly IUserService _userService;
		readonly IConfiguration _configuration;
		readonly ILogger _logger;

		public AccountController(IUnitOfWork unitOfWork, IUserService userService, IConfiguration configuration, ILogger<AccountController> logger) {
			_unitOfWork = unitOfWork;
			_userService = userService;
			_configuration = configuration;
			_logger = logger;
		}

		[HttpPost("reg")]
		public ActionResult Register(RegistrationRequest request) {
			if (!ModelState.IsValid) return BadRequest(new { message = "Данные не удовлетворяют требованиям" });

			try {
				_userService.Register(request.Name, request.Login, request.Password, request.ConfirmPassword);
			} catch (DuplicateLoginException) {
				return BadRequest(new { message = "Пользователь с таким e-mail уже зарегистрирован в системе" });
			} catch (ArgumentException e) {
				switch (e.ParamName) {
				case "password":
					return BadRequest(new { message = "Пароль не удовлетворяет требованиям безопасности" });
				}
				throw;
			}

			_unitOfWork.Complete();

			return Ok();
		}

		string GenerateJwtToken(UserModel user) {
			var claims = new[] {
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(JwtRegisteredClaimNames.Sub, user.Login),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var expires = DateTime.Now.AddDays(1);

			var token = new JwtSecurityToken(
				_configuration["JwtIssuer"],
				_configuration["JwtIssuer"],
				claims,
				expires: expires,
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		[HttpPost("login")]
		public ActionResult<string> Login(AuthenticationRequest request) {
			UserModel user;

			try {
				user = _userService.Authenticate(request.Login, request.Password);
			} catch (UserNotFoundException) {
				return BadRequest(new { message = "Неверные e-mail пользователя или пароль" });
			} catch (ArgumentException e) {
				switch (e.ParamName) {
				case "password":
					return BadRequest(new { message = "Неверные e-mail пользователя или пароль" });
				}
				throw;
			}

			string jwtToken = GenerateJwtToken(user);
			return Ok(jwtToken);
		}
	}
}
