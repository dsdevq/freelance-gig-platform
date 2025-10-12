using JobService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobService.Infrastructure.Configurations;

public class JobConfiguration : IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        builder.ToTable("Jobs");
        
        builder.HasKey(j => j.Id);
        
        builder.Property(j => j.Title)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(j => j.Description)
            .IsRequired()
            .HasMaxLength(5000);
        
        builder.Property(j => j.Budget)
            .IsRequired()
            .HasPrecision(18, 2);
        
        builder.Property(j => j.Status)
            .IsRequired()
            .HasConversion<string>();
        
        builder.Property(j => j.ClientId)
            .IsRequired();
        
        builder.Property(j => j.Category)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(j => j.RequiredSkills)
            .IsRequired();
        
        builder.Property(j => j.EstimatedDurationInDays)
            .IsRequired();
        
        builder.Property(j => j.CreatedAt)
            .IsRequired();
        
        builder.Property(j => j.UpdatedAt)
            .IsRequired();
        
        builder.HasIndex(j => j.ClientId);
        builder.HasIndex(j => j.FreelancerId);
        builder.HasIndex(j => j.Status);
        builder.HasIndex(j => j.Category);
    }
}

