using DBetter.Domain.ServiceCategories;
using DBetter.Domain.ServiceCategories.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBetter.Infrastructure.Postgres;

public class ServiceCategoryMapping : IEntityTypeConfiguration<ServiceCategory>
{
    public void Configure(EntityTypeBuilder<ServiceCategory> builder)
    {
        builder.ToTable("ServiceCategories");
        
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.ShortName)
            .IsUnique();

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new ServiceCategoryId(value));

        builder.Property(x => x.ShortName);

        builder.Property(x => x.Name);

        builder.Property(x => x.CateringExpected);

        builder.Property(x => x.UsesServiceNumberForPassengers);
    }
}