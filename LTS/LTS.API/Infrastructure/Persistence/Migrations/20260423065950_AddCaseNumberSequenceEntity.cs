using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LTS.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCaseNumberSequenceEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CNIC",
                table: "Petitioners",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CaseNumberSequences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    LastSequence = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseNumberSequences", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Petitioners_CNIC",
                table: "Petitioners",
                column: "CNIC",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaseNumberSequences");

            migrationBuilder.DropIndex(
                name: "IX_Petitioners_CNIC",
                table: "Petitioners");

            migrationBuilder.DropColumn(
                name: "CNIC",
                table: "Petitioners");
        }
    }
}
