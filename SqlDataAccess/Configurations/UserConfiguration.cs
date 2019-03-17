using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnaDron.SDTest.SqlDataAccess {
	sealed class UserConfiguration:IEntityTypeConfiguration<User> {
		public void Configure(EntityTypeBuilder<User> builder) {
			builder.Property(e => e.Name).IsRequired().HasMaxLength(30);
			builder.Property(e => e.Login).IsRequired().HasMaxLength(50);
			builder.Property(e => e.Password).IsRequired().HasMaxLength(20);
		}
	}
}
