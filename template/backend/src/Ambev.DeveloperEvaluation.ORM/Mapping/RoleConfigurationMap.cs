using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.RegularExpressions;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

//public class RoleConfigurationMap : IEntityTypeConfiguration<Role>
//{
//    public void Configure(EntityTypeBuilder<Role> builder)
//    {
//        builder.ToTable("Roles");
//        builder.HasKey(u => u.Id);
//        builder.HasData(
//            new Role { Id = new Guid(), Description = "Customer" },
//            new Role { Id = new Guid(), Description = "Manager" },
//            new Role { Id = new Guid(), Description = "Admin" }
//        );
//    }
//}
