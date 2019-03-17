using System;
using AnaDron.SDTest.Model;
using AnaDron.SDTest.SqlDataAccess;
using Autofac;

namespace AnaDron.SDTest {
	class Program {
		static void Main(string[] args) {
			var builder = new ContainerBuilder();

			builder.RegisterModule(new ModelModule());
			builder.RegisterModule(new SqlDataAccessModule());

			var container = builder.Build();

			var unitOfWork = container.Resolve<IUnitOfWork>();
			var userService = container.Resolve<IUserService>();
			var transactionService = container.Resolve<ITransactionService>();

			SeedData.Initialize(unitOfWork, userService, transactionService);

			foreach (var user in unitOfWork.Users.GetAll()) {
				Console.WriteLine($"User '{user.Name}':");
				var transactions = transactionService.GetLast(user.Id, 10);
				foreach (var transaction in transactions)
					Console.WriteLine($"\t{transaction.DT.ToString("yyyy.MM.dd HH:mm:ss")} '{transaction.Payee.Name}' -> '{transaction.Recepient.Name}' {transaction.Amount}");
			}

			unitOfWork.Dispose();

			container.Dispose();
		}
	}
}
