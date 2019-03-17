using System;
using System.Linq;
using AnaDron.SDTest.Model;
using AnaDron.SDTest.SqlDataAccess;

namespace AnaDron.SDTest {
	static class SeedData {
		public static void Initialize(IUnitOfWork unitOfWork, IUserService userService, ITransactionService transactionService) {
			if (unitOfWork.Users.Any()) return;

			void UserAdd(string name, string domain = "example.com", string password = "Aa1234", double balance = 500) {
				var login = $"{name.Replace(" ", "")}@{domain}";
				userService.Register(name, login, password, password, balance);
			}

			UserAdd("Kyle Porter");
			UserAdd("Tommy Stevens");
			UserAdd("Jordan Hart");
			UserAdd("Walter White", "bb.com", "C10h15N", 1000000);
			UserAdd("Samuel Knight");
			UserAdd("Tyler Dean");
			UserAdd("Jordan Briggs");
			UserAdd("Maddox Hewitt");
			UserAdd("Shawn Lindsey");
			UserAdd("Jaden Cote");
			UserAdd("Jett Benjamin");

			unitOfWork.Complete();

			var users = unitOfWork.Users.GetAll().ToDictionary(x => x.Name, x => x.Id);

			void TransactionAdd(string dtString, string payeeName, string recepientName, double amount) {
				var dt = DateTime.Parse(dtString);
				var payee = users[payeeName];
				var recepient = users[recepientName];
				transactionService.Add(dt, payee, recepient, amount);
			}

			TransactionAdd("2019.03.05 12:00:00", "Kyle Porter", "Tommy Stevens", 200);
			TransactionAdd("2019.03.05 12:30:00", "Kyle Porter", "Jordan Hart", 200);
			TransactionAdd("2019.03.08 09:30:00", "Walter White", "Kyle Porter", 1000);
			TransactionAdd("2019.03.09 12:00:00", "Kyle Porter", "Tyler Dean", 200);
			TransactionAdd("2019.03.09 12:30:00", "Kyle Porter", "Jordan Briggs", 200);
			TransactionAdd("2019.03.10 17:45:00", "Shawn Lindsey", "Walter White", 500);
			TransactionAdd("2019.03.10 17:45:01", "Jaden Cote", "Walter White", 500);
			TransactionAdd("2019.03.10 17:45:02", "Jett Benjamin", "Walter White", 500);
			TransactionAdd("2019.03.12 11:00:00", "Maddox Hewitt", "Tyler Dean", 50);

			unitOfWork.Complete();
		}
	}
}
