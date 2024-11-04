using DBetter.Domain.Users;
using DBetter.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBetter.Infrastructure.Postgres;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        
        builder.HasKey(user => user.Id);

        builder.Property(user => user.Id)
            .HasConversion(
                id => id.Value,
                value => new UserId(value));
        
        builder.Property(user => user.Firstname)
            .HasConversion(
                firstname => firstname.Value,
                value => Firstname.Create(value).Value);
        
        builder.Property(user => user.Lastname)
            .HasConversion(
                lastname => lastname.Value,
                value => Lastname.Create(value).Value);

        builder.Property(user => user.Email)
            .HasConversion(
                email => email.Value,
                value => Email.Create(value).Value);
        
        builder.Property(user => user.Birthday)
            .HasConversion(
                birthday => birthday.Utc,
                value => Birthday.Create(value).Value);

        builder.Property("_passwordHash")
            .HasColumnName("PasswordHash")
            .IsRequired();

        builder.Property("_passwordSalt")
            .HasColumnName("PasswordSalt")
            .IsRequired();

        builder.ComplexProperty<RefreshToken>("_refreshToken")
            .IsRequired();

        builder.Ignore(user => user.Discounts);
        builder.Ignore(user => user.CurrentDiscounts);

        builder.OwnsMany<Discount>("_discounts", discountBuilder =>
        {
            discountBuilder.ToTable("Discounts");
            
            discountBuilder.WithOwner().HasForeignKey("UserId");

            discountBuilder.Property<Guid>("Id");
            discountBuilder.HasKey("Id");

            discountBuilder.Property(discount => discount.Type);
            discountBuilder.Property(discount => discount.Class);
            discountBuilder.Property(discount => discount.BoughtAtUtc);
            discountBuilder.Property(discount => discount.ValidUntilUtc);
        });
    }
}