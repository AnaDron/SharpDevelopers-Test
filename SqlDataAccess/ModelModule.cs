using AnaDron.SDTest.Model;
using Autofac;

namespace AnaDron.SDTest.SqlDataAccess {
	public sealed class SqlDataAccessModule:Module {
		protected override void Load(ContainerBuilder builder) {
			builder.RegisterType<ConnectionStringProvider>().As<IConnectionStringProvider>();
			builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
		}
	}
}
