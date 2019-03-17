using System;

namespace AnaDron.SDTest.Model {
	public sealed class TransactionModel {
		internal TransactionModel(ITransactionDTO source) {
			Id = source.Id;
			DT = source.DT;
			Payee = source.Payee.ToModel();
			Recepient = source.Recepient.ToModel();
			Amount = source.Amount;
		}

		public int Id { get; }
		public DateTime DT { get; }
		public UserModel Payee { get; }
		public UserModel Recepient { get; }
		public double Amount { get; }
	}
}
