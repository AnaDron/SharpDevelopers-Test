using System.ComponentModel.DataAnnotations;

namespace AnaDron.SDTest.WebApi.Requests {
	public sealed class TransactionsGetLastRequest {
		[Range(5, 100)]
		public int? Limit { get; set; }
	}
}