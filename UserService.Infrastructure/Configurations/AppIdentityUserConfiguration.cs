using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Infrastructure.Entities;

namespace UserService.Infrastructure.Configurations;

public class AppIdentityUserConfiguration : IEntityTypeConfiguration<AppIdentityUser>
{
    public void Configure(EntityTypeBuilder<AppIdentityUser> builder)
    {
        builder.ToTable("Users");

        builder.Property(u => u.Email)
            .HasMaxLength(255)
            .IsRequired();
    }
}