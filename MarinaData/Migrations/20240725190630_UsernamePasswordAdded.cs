using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarinaData.Migrations
{
    /// <inheritdoc />
    public partial class UsernamePasswordAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Customer",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Customer",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Customer",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "Password", "Username" },
                values: new object[] { "password", "jdoe" });

            migrationBuilder.UpdateData(
                table: "Customer",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "Password", "Username" },
                values: new object[] { "password", "swilliams" });

            migrationBuilder.UpdateData(
                table: "Customer",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "Password", "Username" },
                values: new object[] { "password", "kwong" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Customer");
        }
    }
}
