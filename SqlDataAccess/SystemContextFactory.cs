using Microsoft.EntityFrameworkCore.Design;

namespace AnaDron.SDTest.SqlDataAccess {
	sealed class SystemContextFactory:IDesignTimeDbContextFactory<SystemContext> {
		public SystemContext CreateDbContext(string[] args) {
			return new SystemContext(new ConnectionStringProvider());
		}
	}
}
