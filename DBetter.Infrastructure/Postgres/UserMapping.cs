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
        
        builder.Property(user => user.Password)
            .HasConversion(
                password => password.Value,
                value => Password.Create(value).Value);
    }
}