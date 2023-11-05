using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

        builder.HasMany(c => c.Employees)
            .WithMany(e => e.Companies)
            .UsingEntity(
                l => l.HasOne(typeof(Employee)).WithMany().OnDelete(DeleteBehavior.Restrict),
                r => r.HasOne(typeof(Company)).WithMany().OnDelete(DeleteBehavior.Restrict));
    }
}