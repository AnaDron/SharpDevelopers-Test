using AnaDron.SDTest.Model;

namespace AnaDron.SDTest.SqlDataAccess {
	sealed class UnitOfWork:IUnitOfWork {
		readonly SystemContext _systemContext;

		public UnitOfWork(IConnectionStringProvider connectionStringProvider) {
			_systemContext = new SystemContext(connectionStringProvider);

			Users = new SqlUserRepository(_systemContext);
			Transactions = new SqlTransactionRepository(_systemContext);
		}

		public void Dispose() {
			_systemContext.Dispose();
		}

		public IUserRepository Users { get; }

		public ITransactionRepository Transactions { get; }

		public int Complete() {
			return _systemContext.SaveChanges();
		}
	}
}
