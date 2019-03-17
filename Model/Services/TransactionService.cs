using System;
using System.Collections.Generic;
using System.Linq;

namespace AnaDron.SDTest.Model {
	sealed class TransactionService:ServiceBase, ITransactionService {
		public TransactionService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

		public IEnumerable<TransactionModel> GetLast(int userId, int count) {
			Users.CheckExist(userId);
			if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count), "Count must be positive number");

			return Transactions.GetLast(userId, count).Select(x => x.ToModel());
		}

		public void Add(DateTime dt, int payeeId, int recepientId, double amount) {
			if (recepientId == payeeId) throw new ArgumentException("Recepient and payee must not be the same", nameof(recepientId));
			var payee = Users.GetExist(payeeId, "Payee");
			var recepient = Users.GetExist(recepientId, "Recepient");
			if (amount > payee.Balance) throw new ArgumentException("Balance of payee less then amount", nameof(amount));

			Transactions.Add(dt, payeeId, recepientId, amount);

			payee.Balance -= amount;
			recepient.Balance += amount;

			Users.UpdateBalance(payee);
			Users.UpdateBalance(recepient);
		}
	}
}