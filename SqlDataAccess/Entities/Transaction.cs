using System;
using AnaDron.SDTest.Model;

namespace AnaDron.SDTest.SqlDataAccess {
	sealed class Transaction:ITransactionDTO {
		public int TransactionId { get; set; }

		public DateTime DT { get; set; }

		public int PayeeId { get; set; }
		public User Payee { get; set; }

		public int RecepientId { get; set; }
		public User Recepient { get; set; }

		public double Amount { get; set; }

		int ITransactionDTO.Id => TransactionId;
		IUserDTO ITransactionDTO.Payee => Payee;
		IUserDTO ITransactionDTO.Recepient => Recepient;
	}
}
