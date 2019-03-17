using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnaDron.SDTest.SqlDataAccess {
	sealed class TransactionConfiguration:IEntityTypeConfiguration<Transaction> {
		public void Configure(EntityTypeBuilder<Transaction> builder) {
			builder.HasOne(x => x.Payee)
				.WithMany(x => x.OutputTransactions)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(x => x.Recepient)
				.WithMany(x => x.InputTransactions)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
