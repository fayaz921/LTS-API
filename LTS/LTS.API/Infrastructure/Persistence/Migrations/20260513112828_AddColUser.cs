using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LTS.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddColUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Phonenumber",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phonenumber",
                table: "Users");
        }
    }
}
