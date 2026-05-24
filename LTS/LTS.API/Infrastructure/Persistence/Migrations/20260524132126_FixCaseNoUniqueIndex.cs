using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LTS.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixCaseNoUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cases_CaseNo",
                table: "Cases");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_OrganizationId_CaseNo",
                table: "Cases",
                columns: new[] { "OrganizationId", "CaseNo" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cases_OrganizationId_CaseNo",
                table: "Cases");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_CaseNo",
                table: "Cases",
                column: "CaseNo",
                unique: true);
        }
    }
}
