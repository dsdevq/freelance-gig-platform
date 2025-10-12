using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Outbox.Entities;

namespace Shared.Outbox.Configurations;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OutboxMessages");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Type)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Content)
            .IsRequired();

        builder.Property(x => x.OccurredOnUtc)
            .IsRequired();

        builder.HasIndex(x => x.ProcessedOnUtc);
        builder.HasIndex(x => x.OccurredOnUtc);
    }
}

