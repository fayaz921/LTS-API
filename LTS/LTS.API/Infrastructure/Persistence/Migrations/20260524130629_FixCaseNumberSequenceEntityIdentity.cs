using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LTS.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixCaseNumberSequenceEntityIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CaseNumberSequences",
                table: "CaseNumberSequences");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CaseNumberSequences");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CaseNumberSequences",
                table: "CaseNumberSequences",
                column: "OrganizationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CaseNumberSequences",
                table: "CaseNumberSequences");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CaseNumberSequences",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CaseNumberSequences",
                table: "CaseNumberSequences",
                column: "Id");
        }
    }
}
