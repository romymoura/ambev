using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM.Mapping;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class AddFakeData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Criando produtos, obs: Irei criar apenas um rating para todos os produtos mas o ideal é que cada um tenha o seu.
            // para isso podemos colocar a FK como unique, mas não foi o caso.

            migrationBuilder.InsertData(
                table: "Ratings",
                columns: new[] { "Id", "Rate", "Count", "CreatedAt" },
                values: new object[,]
                {
                        { Guid.Parse("862e08eb-fa5b-4393-9e82-983002538378"), 4.5, 100, DateTime.UtcNow },
                });

            var products = AddFakeData.GenerateProductsFake(40);
            foreach (var product in products)
            {
                migrationBuilder.InsertData(
                    table: "Products",
                    columns: new[] { "Id", "Title", "Price", "Description", "Category", "Image", "CreatedAt", "UpdatedAt", "RatingId" },
                    values: new object[] {
                        product.Id, product.Title, product.Price, product.Description, product.Category, product.Image, product.CreatedAt, null, "862e08eb-fa5b-4393-9e82-983002538378"
                    });
            }


            //Usuários iniciais do sistema
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Username", "Email", "Phone", "CreatedAt", "UpdatedAt", "Password", "Role", "Status" },
                values: new object[,]
                {
                    { Guid.NewGuid(), "User Manager", "manager@tsc.com", "11 11111-1111", DateTime.UtcNow, null, BCrypt.Net.BCrypt.HashPassword("@Manager123"), (int)Domain.Enums.UserRole.Manager, (int)Domain.Enums.UserStatus.Active},
                    { Guid.NewGuid(), "User Admin", "admin@tsc.com", "11 11111-1111", DateTime.UtcNow, null, BCrypt.Net.BCrypt.HashPassword("@Admin123"), (int)Domain.Enums.UserRole.Admin, (int)Domain.Enums.UserStatus.Active},
                    { Guid.NewGuid(), "User Manager", "customer@tsc.com", "11 11111-1111", DateTime.UtcNow, null, BCrypt.Net.BCrypt.HashPassword("@Customer123"), (int)Domain.Enums.UserRole.Customer, (int)Domain.Enums.UserStatus.Active},
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Removendo os dados inseridos
            migrationBuilder.Sql($"DELETE FROM Ratings where id = '862e08eb-fa5b-4393-9e82-983002538378'");
            migrationBuilder.Sql("DELETE FROM Products");
            migrationBuilder.Sql("DELETE FROM Users where email in ('manager@tsc.com', 'admin@tsc.com', 'customer@tsc.com')");
        }


        public static List<Product> GenerateProductsFake(int count)
        {
            List<Product> products = new();
            Random random = new();
            Guid fixedRatingId = Guid.Parse("862e08eb-fa5b-4393-9e82-983002538378");
            string[] imageUrls = new string[]
            {
            "https://images.pexels.com/photos/298863/pexels-photo-298863.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
            "https://images.pexels.com/photos/934063/pexels-photo-934063.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
            "https://images.pexels.com/photos/1148957/pexels-photo-1148957.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
            "https://images.pexels.com/photos/1176618/pexels-photo-1176618.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
            "https://images.pexels.com/photos/1082528/pexels-photo-1082528.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
            "https://images.pexels.com/photos/581087/pexels-photo-581087.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
            "https://images.pexels.com/photos/934069/pexels-photo-934069.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
            "https://images.pexels.com/photos/297933/pexels-photo-297933.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
            "https://images.pexels.com/photos/2081332/pexels-photo-2081332.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
            "https://images.pexels.com/photos/135620/pexels-photo-135620.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
            };

            for (int i = 1; i <= count; i++)
            {
                products.Add(new Product
                {
                    Id = Guid.NewGuid(),
                    Title = $"Produto {i}",
                    Price = (decimal)Math.Round(random.Next(10, 500) + random.NextDouble(), 2),
                    Description = $"Descrição do Produto {i}",
                    Category = $"Categoria {random.Next(1, 5)}",
                    Image = imageUrls[random.Next(imageUrls.Length)],
                    Rating = new Rating
                    {
                        Id = fixedRatingId,
                        Rate = Math.Round(random.NextDouble() * 5, 1), // Gera uma nota entre 0.0 e 5.0
                        Count = random.Next(1, 500) // Gera um número aleatório de avaliações
                    }
                });
            }
            return products;
        }
    }
}
