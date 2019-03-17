using System;
using System.Collections.Generic;

namespace AnaDron.SDTest.Model {
	public interface ITransactionService {
		IEnumerable<TransactionModel> GetLast(int userId, int count);

		void Add(DateTime dt, int payeeId, int recepientId, double amount);
	}
}