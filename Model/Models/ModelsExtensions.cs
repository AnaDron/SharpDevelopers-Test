namespace AnaDron.SDTest.Model {
	static class ModelsExtensions {
		public static UserModel ToModel(this IUserDTO self) => new UserModel(self);
		public static TransactionModel ToModel(this ITransactionDTO self) => new TransactionModel(self);
	}
}