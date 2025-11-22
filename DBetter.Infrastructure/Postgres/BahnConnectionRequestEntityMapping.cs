using System.Text.Json;
using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections.Entities;
using DBetter.Infrastructure.BahnDe.Connections.Parameters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DBetter.Infrastructure.Postgres;

public class BahnConnectionRequestEntityMapping : IEntityTypeConfiguration<BahnConnectionRequestEntity>
{
    public void Configure(EntityTypeBuilder<BahnConnectionRequestEntity> builder)
    {
        builder.ToTable("BahnConnectionRequests");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new ConnectionRequestId(value));

        builder.Property(x => x.Request)
            .HasConversion(
                request => JsonSerializer.Serialize(request,
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
                value => JsonSerializer.Deserialize<ReiseAnfrage>(value,
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })!)
            .HasColumnType("jsonb");
    }
}