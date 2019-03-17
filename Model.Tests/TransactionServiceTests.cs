using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Moq;
using NUnit.Framework;

namespace AnaDron.SDTest.Model.Tests {
	public class TransactionServiceTests {
		ContainerBuilder _builder;
		Mock<IUserRepository> _mockUserRepository;
		Mock<ITransactionRepository> _mockTransactionRepository;

		[SetUp]
		public void Setup() {
			_builder = new ContainerBuilder();

			_builder.RegisterModule(new ModelModule());

			var stubUnitOfWork = new Mock<IUnitOfWork>();
			_builder.RegisterInstance(stubUnitOfWork.Object).As<IUnitOfWork>();

			_mockUserRepository = new Mock<IUserRepository>();
			stubUnitOfWork.Setup(x => x.Users).Returns(_mockUserRepository.Object);

			_mockTransactionRepository = new Mock<ITransactionRepository>();
			stubUnitOfWork.Setup(x => x.Transactions).Returns(_mockTransactionRepository.Object);
		}

		[TearDown]
		public void TearDown() {
			_builder = null;
			_mockUserRepository = null;
			_mockTransactionRepository = null;
		}

		[Test]
		public void AddNormal() {
			var transaction = new TransactionDTO {
				DT = new DateTime(2019, 01, 10, 12, 30, 41),
				Payee = new UserDTO { Id = 1, Balance = 100 },
				Recepient = new UserDTO { Id = 2, Balance = 0 },
				Amount = 10
			};

			_mockUserRepository.Setup(x => x.Get(transaction.Payee.Id)).Returns(transaction.Payee);
			_mockUserRepository.Setup(x => x.Get(transaction.Recepient.Id)).Returns(transaction.Recepient);

			using (var container = _builder.Build()) {
				var transactionService = container.Resolve<ITransactionService>();

				transactionService.Add(transaction.DT, transaction.Payee.Id, transaction.Recepient.Id, transaction.Amount);
			}

			_mockUserRepository.Verify(x => x.Get(It.IsAny<int>()), Times.Exactly(2));
			_mockUserRepository.Verify(x => x.Get(transaction.Payee.Id), Times.Once());
			_mockUserRepository.Verify(x => x.Get(transaction.Recepient.Id), Times.Once());

			_mockTransactionRepository.Verify(x => x.Add(transaction.DT, transaction.Payee.Id, transaction.Recepient.Id, transaction.Amount), Times.Once());

			Assert.AreEqual(transaction.Payee.Balance, 90);
			Assert.AreEqual(transaction.Recepient.Balance, 10);
		}

		[Test]
		public void AddPayeeIdWrong() {
			using (var container = _builder.Build()) {
				var transactionService = container.Resolve<ITransactionService>();

				var e = Assert.Throws<UserNotFoundException>(
					delegate { transactionService.Add(-1, 1, 1); }
				);

				Assert.AreEqual(e.Name, "Payee");
			}
		}

		[Test]
		public void AddRecepientIdWrong() {
			_mockUserRepository.Setup(x => x.Get(1)).Returns(new UserDTO());
			using (var container = _builder.Build()) {
				var transactionService = container.Resolve<ITransactionService>();

				var e = Assert.Throws<UserNotFoundException>(
					delegate { transactionService.Add(1, -1, 1); }
				);

				Assert.AreEqual(e.Name, "Recepient");
			}
		}

		[Test]
		public void AddPayeeIdSameAsRecepientId() {
			using (var container = _builder.Build()) {
				var transactionService = container.Resolve<ITransactionService>();

				var e = Assert.Throws<ArgumentException>(
					delegate { transactionService.Add(1, 1, 1); }
				);

				Assert.AreEqual(e.ParamName, "recepientId");
			}
		}

		[Test]
		public void AddAmountLessBalance() {
			_mockUserRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new UserDTO { Balance = 10 });
			using (var container = _builder.Build()) {
				var transactionService = container.Resolve<ITransactionService>();

				var e = Assert.Throws<ArgumentException>(
					delegate { transactionService.Add(1, 2, 100); }
				);

				Assert.AreEqual(e.ParamName, "amount");
			}
		}

		[Test]
		public void GetLastNormal() {
			var user = new UserDTO { Id = 1 };
			_mockUserRepository.Setup(x => x.Get(user.Id)).Returns(user);

			var transactions = Enumerable.Repeat(new TransactionDTO { Payee = new UserDTO(), Recepient = new UserDTO() }, 5).ToList();
			_mockTransactionRepository.Setup(x => x.GetLast(user.Id, 10)).Returns(transactions);

			using (var container = _builder.Build()) {
				var transactionService = container.Resolve<ITransactionService>();

				var result = transactionService.GetLast(user.Id, 10);

				Assert.AreEqual(result.Count(), transactions.Count);
			}

			_mockUserRepository.Verify(x => x.Get(It.IsAny<int>()), Times.Once());
			_mockUserRepository.Verify(x => x.Get(user.Id), Times.Once());

			_mockTransactionRepository.Verify(x => x.GetLast(user.Id, 10), Times.Once());
		}

		[Test]
		public void GetLastUserIdWrong() {
			using (var container = _builder.Build()) {
				var transactionService = container.Resolve<ITransactionService>();

				Assert.Throws<UserNotFoundException>(
					delegate { transactionService.GetLast(-1, 10); }
				);
			}
		}

		[Test]
		public void GetLastCountNotPositive() {
			_mockUserRepository.Setup(x => x.Get(It.IsAny<int>())).Returns(new UserDTO());
			using (var container = _builder.Build()) {
				var transactionService = container.Resolve<ITransactionService>();

				void Test(int count) {
					var e = Assert.Throws<ArgumentOutOfRangeException>(
						delegate { transactionService.GetLast(1, count); }
					);

					Assert.AreEqual(e.ParamName, "count");
				}

				Test(0);
				Test(-1);
			}
		}
	}
}