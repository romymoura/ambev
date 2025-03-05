using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class RatingConfigurationMap : IEntityTypeConfiguration<Rating>
{
    public void Configure(EntityTypeBuilder<Rating> builder)
    {
        // Configuração da tabela
        builder.ToTable("Ratings");

        // Chave primária
        builder.HasKey(x => x.Id);
        builder.Property(u => u.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(r => r.Rate)
               .IsRequired()
               .HasColumnName("RatingRate")
               .HasColumnType("decimal(3,1)");

        builder.Property(r => r.Count)
            .IsRequired()
            .HasColumnName("RatingCount");

        builder.Property(u => u.CreatedAt).HasDefaultValue(DateTime.Now);
        builder.Property(u => u.UpdatedAt);
    }
}
