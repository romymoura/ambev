using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.RegularExpressions;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

//public class UserRoleConfigurationMap : IEntityTypeConfiguration<UserRole>
//{
//    public void Configure(EntityTypeBuilder<UserRole> builder)
//    {
//        builder.ToTable("UserRoles");
//        builder.HasKey(u => u.Id);
//        builder.Property(u => u.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");
//        //builder.HasOne(u => u.User)
//        //    .WithMany(ur => ur.Roles)
//        //    .HasForeignKey(u => u.IdRole);
//    }
//}
