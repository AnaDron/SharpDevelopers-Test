using System;
using Autofac;
using NUnit.Framework;

namespace AnaDron.SDTest.Model.Tests {
	public class PasswordValidatorTests {
		const string VeriefedPassword = "Passw0rd";

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
				var passwordValidator = container.Resolve<IPasswordValidator>();

				passwordValidator.Validate(VeriefedPassword);
			}
		}

		[Test]
		public void ValidateNull() {
			using (var container = _builder.Build()) {
				var passwordValidator = container.Resolve<IPasswordValidator>();

				var e = Assert.Throws<ArgumentNullException>(
					delegate { passwordValidator.Validate(null); }
				);

				Assert.AreEqual(e.ParamName, "password");
			}
		}

		[Test]
		public void ValidateInvalid() {
			using (var container = _builder.Build()) {
				var passwordValidator = container.Resolve<IPasswordValidator>();

				void Test(string login) {
					var e = Assert.Throws<ArgumentException>(
						delegate { passwordValidator.Validate(login); }
					);

					Assert.AreEqual(e.ParamName, "password");
				}

				Test("P0ssw"); // length < 6
				Test("passw0rd"); // don't contain capital letter
				Test("Password"); // don't contain digit
			}
		}
	}
}