using System.ComponentModel.DataAnnotations;

namespace AnaDron.SDTest.WebApi.Requests {
	public sealed class TransactionsAddRequest {
		[Required]
		public int ContragentId { get; set; }

		[Required]
		[Range(1, double.MaxValue)]
		public double Amount { get; set; }
	}
}