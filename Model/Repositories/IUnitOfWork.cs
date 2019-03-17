using System;

namespace AnaDron.SDTest.Model {
	public interface IUnitOfWork:IDisposable {
		IUserRepository Users { get; }
		ITransactionRepository Transactions { get; }

		int Complete();
	}
}