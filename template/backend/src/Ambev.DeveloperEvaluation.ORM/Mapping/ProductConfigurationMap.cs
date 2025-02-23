using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class ProductConfigurationMap : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Configuração da tabela
        builder.ToTable("Products");

        // Chave primária
        builder.HasKey(x => x.Id);
        builder.Property(u => u.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        // Propriedades
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Category)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Image)
            .IsRequired()
            .HasMaxLength(500);

        // Configuração do objeto Rating (Owned Entity)
        builder.OwnsOne(x => x.Rating);

        // Índices
        builder.HasIndex(x => x.Category);

        builder.Property(u => u.CreatedAt).HasDefaultValue(DateTime.Now);
        builder.Property(u => u.UpdatedAt);
    }
}
