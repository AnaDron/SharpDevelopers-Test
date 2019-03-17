using System;
using System.Collections.Generic;
using System.Linq;
using AnaDron.SDTest.Model;
using Microsoft.EntityFrameworkCore;

namespace AnaDron.SDTest.SqlDataAccess {
	sealed class SqlTransactionRepository:ITransactionRepository {
		readonly SystemContext _context;

		public SqlTransactionRepository(SystemContext context) {
			_context = context;
		}

		public IEnumerable<ITransactionDTO> GetLast(int userId, int count) => _context.Transactions
			.Where(x => x.PayeeId == userId || x.RecepientId == userId)
			.OrderByDescending(x => x.DT)
			.Include(x => x.Payee)
			.Include(x => x.Recepient)
			.ToList();

		public void Add(DateTime dt, int payeeId, int recepientId, double amount) {
			_context.Transactions.Add(new Transaction {
				DT = dt,
				PayeeId = payeeId,
				RecepientId = recepientId,
				Amount = amount
			});
		}
	}
}