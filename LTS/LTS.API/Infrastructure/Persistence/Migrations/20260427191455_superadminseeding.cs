using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LTS.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class superadminseeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:case_status", "pending,finalized")
                .Annotation("Npgsql:Enum:user_role", "super_admin,admin,user")
                .OldAnnotation("Npgsql:Enum:case_status", "pending,finalized")
                .OldAnnotation("Npgsql:Enum:user_role", "admin,user");

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                table: "Departments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Departments");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:case_status", "pending,finalized")
                .Annotation("Npgsql:Enum:user_role", "admin,user")
                .OldAnnotation("Npgsql:Enum:case_status", "pending,finalized")
                .OldAnnotation("Npgsql:Enum:user_role", "super_admin,admin,user");
        }
    }
}
