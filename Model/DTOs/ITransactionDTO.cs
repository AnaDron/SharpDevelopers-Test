using System;

namespace AnaDron.SDTest.Model {
	public interface ITransactionDTO {
		int Id { get; }
		DateTime DT { get; }
		IUserDTO Payee { get; }
		IUserDTO Recepient { get; }
		double Amount { get; }
	}
}
