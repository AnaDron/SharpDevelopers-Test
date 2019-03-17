using System.Collections.Generic;
using AnaDron.SDTest.Model;

namespace AnaDron.SDTest.SqlDataAccess {
	sealed class User:IUserDTO {
		public int UserId { get; set; }

		public string Login { get; set; }
		public string Password { get; set; }

		public string Name { get; set; }

		public double Balance { get; set; }

		public List<Transaction> InputTransactions { get; set; }
		public List<Transaction> OutputTransactions { get; set; }

		int IUserDTO.Id => UserId;
	}
}
