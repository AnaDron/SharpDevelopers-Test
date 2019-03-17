using System;

namespace AnaDron.SDTest.Model {
	public static class ServiceExtensions {
		public static double StartBalance = 500;

		public static void Register(this IUserService self, string name, string login, string password, string passwordConfirm) {
			self.Register(name, login, password, passwordConfirm, StartBalance);
		}

		public static void Add(this ITransactionService self, int payeeId, int recepientId, double amount) {
			self.Add(DateTime.Now, payeeId, recepientId, amount);
		}

		internal static IUserDTO Exist(this IUserDTO self, string alias = "User") => self ?? throw new UserNotFoundException(alias);

		internal static IUserDTO GetExist(this IUserRepository self, int id, string alias = "User") => self.Get(id).Exist(alias);

		internal static void CheckExist(this IUserRepository self, int id, string alias = "User") => self.GetExist(id, alias);
	}
}