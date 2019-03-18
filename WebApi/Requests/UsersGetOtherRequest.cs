using System.ComponentModel.DataAnnotations;

namespace AnaDron.SDTest.WebApi.Requests {
	public sealed class UsersGetOtherRequest {
		public string Template { get; set; }

		[Range(1, 10)]
		public int? Limit { get; set; }
	}
}