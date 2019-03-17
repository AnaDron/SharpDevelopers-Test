using AnaDron.SDTest.Model;
using Microsoft.EntityFrameworkCore;

namespace AnaDron.SDTest.SqlDataAccess {
	sealed class SystemContext:DbContext {
		readonly IConnectionStringProvider _connectionStringProvider;

		public SystemContext(IConnectionStringProvider connectionStringProvider) {
			_connectionStringProvider = connectionStringProvider;
		}

		public DbSet<User> Users { get; set; }
		public DbSet<Transaction> Transactions { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			var connectionString = _connectionStringProvider.GetConnectionString();

			optionsBuilder.UseSqlServer(connectionString);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(SystemContext).Assembly);
		}
	}
}
