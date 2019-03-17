using System;
using Autofac;
using NUnit.Framework;

namespace AnaDron.SDTest.Model.Tests {
	public class LoginValidatorTests {
		const string VeriefedLogin = "Login";

		ContainerBuilder _builder;

		[SetUp]
		public void Setup() {
			_builder = new ContainerBuilder();

			_builder.RegisterModule(new ModelModule());
		}

		public void TearDown() {
			_builder = null;
		}

		[Test]
		public void ValidateVerified() {
			using (var container = _builder.Build()) {
				var loginValidator = container.Resolve<ILoginValidator>();

				loginValidator.Validate(VeriefedLogin);
			}
		}

		[Test]
		public void ValidateNull() {
			using (var container = _builder.Build()) {
				var loginValidator = container.Resolve<ILoginValidator>();

				var e = Assert.Throws<ArgumentNullException>(
					delegate { loginValidator.Validate(null); }
				);

				Assert.AreEqual(e.ParamName, "login");
			}
		}

		// [Test]
		// public void ValidateInvalid() {
		// 	using (var container = _builder.Build()) {
		// 		var loginValidator = container.Resolve<ILoginValidator>();

		// 		void Test(string login) {
		// 			var e = Assert.Throws<ArgumentException>(
		// 				delegate { loginValidator.Validate(login); }
		// 			);

		// 			Assert.AreEqual(e.ParamName, "login");
		// 		}

		// 		Test("0login"); // start with digit
		// 		Test("lo"); // length < 3
		// 		Test("LoginLoginLoginLogin"); // length > 16
		// 	}
		// }
	}
}