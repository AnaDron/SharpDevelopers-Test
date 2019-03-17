using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Moq;
using NUnit.Framework;

namespace AnaDron.SDTest.Model.Tests {
	public class UserServiceTests {
		const string VeriefedName = "Name";
		const string VeriefedLogin = "Login";
		const string VeriefedPassword = "Passw0rd";

		ContainerBuilder _builder;
		Mock<IUserRepository> _mockUserRepository;

		[SetUp]
		public void Setup() {
			_builder = new ContainerBuilder();

			_builder.RegisterModule(new ModelModule());

			var stubUnitOfWork = new Mock<IUnitOfWork>();
			_builder.RegisterInstance(stubUnitOfWork.Object).As<IUnitOfWork>();

			_mockUserRepository = new Mock<IUserRepository>();
			stubUnitOfWork.Setup(x => x.Users).Returns(_mockUserRepository.Object);
		}

		[TearDown]
		public void TearDown() {
			_builder = null;
			_mockUserRepository = null;
		}

		[Test]
		public void RegisterNormal() {
			var user = new UserDTO {
				Name = VeriefedName,
				Login = VeriefedLogin,
				Password = VeriefedPassword,
				Balance = 1000000
			};

			using (var container = _builder.Build()) {
				var userService = container.Resolve<IUserService>();

				userService.Register(user.Name, user.Login, user.Password, user.Password, user.Balance);
			}

			// Verify call FindByLogin once with VeriefedLogin
			_mockUserRepository.Verify(x => x.FindByLogin(It.IsAny<string>()), Times.Once());
			_mockUserRepository.Verify(x => x.FindByLogin(user.Login), Times.Once());

			_mockUserRepository.Verify(x => x.Add(user.Name, user.Login, user.Password, user.Balance), Times.Once());
		}

		[Test]
		public void RegisterUseValidators() {
			var stubLoginValidator = new Mock<ILoginValidator>();
			_builder.RegisterInstance(stubLoginValidator.Object).As<ILoginValidator>();
			var stubPasswordValidator = new Mock<IPasswordValidator>();
			_builder.RegisterInstance(stubPasswordValidator.Object).As<IPasswordValidator>();
			using (var container = _builder.Build()) {
				var userService = container.Resolve<IUserService>();

				userService.Register(VeriefedName, VeriefedLogin, VeriefedPassword, VeriefedPassword);
			}

			stubLoginValidator.Verify(x => x.Validate(It.IsAny<string>()), Times.Once());
			stubLoginValidator.Verify(x => x.Validate(VeriefedLogin), Times.Once());

			stubPasswordValidator.Verify(x => x.Validate(It.IsAny<string>()), Times.Once());
			stubPasswordValidator.Verify(x => x.Validate(VeriefedPassword), Times.Once());
		}

		[Test]
		public void RegisterNameInvalid() {
			using (var container = _builder.Build()) {
				var userService = container.Resolve<IUserService>();

				void Test(string name) {
					var e = Assert.Throws<ArgumentNullException>(
						delegate { userService.Register(name, VeriefedLogin, VeriefedPassword, VeriefedPassword); }
					);

					Assert.AreEqual(e.ParamName, "name");
				}

				Test(null);
				Test("");
				Test(" ");
			}
		}

		[Test]
		public void RegisterPasswordConfirmWrong() {
			using (var container = _builder.Build()) {
				var userService = container.Resolve<IUserService>();

				var e = Assert.Throws<ArgumentException>(
					delegate { userService.Register(VeriefedName, VeriefedLogin, VeriefedPassword, $"{VeriefedPassword}z"); }
				);

				Assert.AreEqual(e.ParamName, "passwordConfirm");
			}
		}

		[Test]
		public void RegisterDuplicate() {
			_mockUserRepository.Setup(x => x.FindByLogin(It.IsAny<string>())).Returns(new UserDTO());
			using (var container = _builder.Build()) {
				var userService = container.Resolve<IUserService>();

				Assert.Throws<DuplicateLoginException>(
					delegate { userService.Register(VeriefedName, VeriefedLogin, VeriefedPassword, VeriefedPassword); }
				);
			}
			_mockUserRepository.Verify(x => x.FindByLogin(It.IsAny<string>()), Times.Once());
			_mockUserRepository.Verify(x => x.Add(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>()), Times.Never());
		}

		[Test]
		public void AuthenticateUserNotFound() {
			using (var container = _builder.Build()) {
				var userService = container.Resolve<IUserService>();

				Assert.Throws<UserNotFoundException>(
					delegate { userService.Authenticate(VeriefedLogin, VeriefedPassword); }
				);
			}
		}

		[Test]
		public void AuthenticatePasswordDoesNotMatch() {
			_mockUserRepository.Setup(x => x.FindByLogin(It.IsAny<string>())).Returns(new UserDTO());
			using (var container = _builder.Build()) {
				var userService = container.Resolve<IUserService>();

				var e = Assert.Throws<ArgumentException>(
					delegate { userService.Authenticate(VeriefedLogin, VeriefedPassword); }
				);

				Assert.AreEqual(e.ParamName, "password");
			}
		}

		[Test]
		public void AuthenticateNormal() {
			var user = new UserDTO { Id = 1, Name = VeriefedName, Login = VeriefedLogin, Password = VeriefedPassword, Balance = 100 };
			_mockUserRepository.Setup(x => x.FindByLogin(user.Login)).Returns(user);
			using (var container = _builder.Build()) {
				var userService = container.Resolve<IUserService>();

				var result = userService.Authenticate(user.Login, user.Password);

				Assert.AreEqual(result.Id, user.Id);
				Assert.AreEqual(result.Name, user.Name);
				Assert.AreEqual(result.Login, user.Login);
				Assert.AreEqual(result.Balance, user.Balance);
			}

			// Verify call FindByLogin once with VeriefedLogin
			_mockUserRepository.Verify(x => x.FindByLogin(It.IsAny<string>()), Times.Once());
			_mockUserRepository.Verify(x => x.FindByLogin(VeriefedLogin), Times.Once());
		}

		[Test]
		public void GetNormal() {
			var user = new UserDTO { Id = 1, Name = VeriefedName, Login = VeriefedLogin, Password = VeriefedPassword, Balance = 100 };
			_mockUserRepository.Setup(x => x.Get(user.Id)).Returns(user);

			using (var container = _builder.Build()) {
				var userService = container.Resolve<IUserService>();

				var result = userService.Get(user.Id);

				Assert.AreEqual(result.Id, user.Id);
				Assert.AreEqual(result.Name, user.Name);
				Assert.AreEqual(result.Login, user.Login);
				Assert.AreEqual(result.Balance, user.Balance);
			}

			_mockUserRepository.Verify(x => x.Get(It.IsAny<int>()), Times.Once());
			_mockUserRepository.Verify(x => x.Get(1), Times.Once());
		}

		[Test]
		public void GetIdWrong() {
			using (var container = _builder.Build()) {
				var userService = container.Resolve<IUserService>();

				Assert.Throws<UserNotFoundException>(
					delegate { userService.Get(-1); }
				);
			}
		}

		[Test]
		public void GetOtherNormal() {
			var user = new UserDTO { Id = 1, Name = VeriefedName, Login = VeriefedLogin, Password = VeriefedPassword, Balance = 100 };
			_mockUserRepository.Setup(x => x.Get(user.Id)).Returns(user);

			var users = Enumerable.Repeat(new UserDTO(), 5).ToList();
			_mockUserRepository.Setup(x => x.GetOther(user.Id, null, It.IsAny<int>())).Returns(users);

			using (var container = _builder.Build()) {
				var userService = container.Resolve<IUserService>();

				var result = userService.GetOther(user.Id);

				Assert.AreEqual(result.Count(), users.Count);
			}

			_mockUserRepository.Verify(x => x.Get(It.IsAny<int>()), Times.Once());
			_mockUserRepository.Verify(x => x.Get(user.Id), Times.Once());

			_mockUserRepository.Verify(x => x.GetOther(user.Id, null, It.IsAny<int>()), Times.Once());
		}

		[Test]
		public void GetOtherIdWrong() {
			using (var container = _builder.Build()) {
				var userService = container.Resolve<IUserService>();

				Assert.Throws<UserNotFoundException>(
					delegate { userService.GetOther(-1); }
				);
			}
		}

		[Test]
		public void GetOtherLimitOutOfRange() {
			_mockUserRepository.Setup(x => x.Get(1)).Returns(new UserDTO());

			using (var container = _builder.Build()) {
				var userService = container.Resolve<IUserService>();

				void Test(int limit) {
					var e = Assert.Throws<ArgumentOutOfRangeException>(
						delegate { userService.GetOther(1, null, limit); }
					);

					Assert.AreEqual(e.ParamName, "limit");
				}

				Test(-1);
				Test(0);
				Test(11);
			}
		}
	}
}