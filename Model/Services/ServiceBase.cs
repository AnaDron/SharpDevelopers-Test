namespace AnaDron.SDTest.Model {
	abstract class ServiceBase {
		protected ServiceBase(IUnitOfWork unitOfWork) {
			UnitOfWork = unitOfWork;
		}

		protected IUnitOfWork UnitOfWork { get; }

		protected IUserRepository Users => UnitOfWork.Users;
		protected ITransactionRepository Transactions => UnitOfWork.Transactions;
	}
}