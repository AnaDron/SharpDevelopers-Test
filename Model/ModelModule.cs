using Autofac;

namespace AnaDron.SDTest.Model {
	public sealed class ModelModule:Module {
		protected override void Load(ContainerBuilder builder) {
			builder.RegisterType<LoginValidator>().As<ILoginValidator>();
			builder.RegisterType<PasswordValidator>().As<IPasswordValidator>();
			builder.RegisterType<UserService>().As<IUserService>();
			builder.RegisterType<TransactionService>().As<ITransactionService>();
		}
	}
}
