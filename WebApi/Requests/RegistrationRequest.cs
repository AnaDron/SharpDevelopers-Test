using System.ComponentModel.DataAnnotations;

namespace AnaDron.SDTest.WebApi.Requests {
	public sealed class RegistrationRequest {
		[Required]
		public string Name { get; set; }

		[Required]
		[EmailAddress]
		public string Login { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}
}