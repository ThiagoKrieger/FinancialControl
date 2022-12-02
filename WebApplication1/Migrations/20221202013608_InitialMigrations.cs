using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserViewModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Balance = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserViewModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionViewModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<float>(type: "real", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserViewModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionViewModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionViewModels_UserViewModels_UserViewModelId",
                        column: x => x.UserViewModelId,
                        principalTable: "UserViewModels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionViewModels_UserViewModelId",
                table: "TransactionViewModels",
                column: "UserViewModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionViewModels");

            migrationBuilder.DropTable(
                name: "UserViewModels");
        }
    }
}
