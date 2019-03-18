using System.ComponentModel.DataAnnotations;

namespace AnaDron.SDTest.WebApi.Requests {
	public sealed class AuthenticationRequest {
		[Required]
		[EmailAddress]
		public string Login { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}