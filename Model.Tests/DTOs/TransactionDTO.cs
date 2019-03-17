using System;

namespace AnaDron.SDTest.Model {
	sealed class TransactionDTO:ITransactionDTO {
		public int Id { get; set; }
		public DateTime DT { get; set; }
		public UserDTO Payee { get; set; }
		public UserDTO Recepient { get; set; }
		public double Amount { get; set; }

		IUserDTO ITransactionDTO.Payee => Payee;
		IUserDTO ITransactionDTO.Recepient => Recepient;
	}
}
