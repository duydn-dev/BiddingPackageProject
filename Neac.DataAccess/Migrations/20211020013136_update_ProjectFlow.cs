using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Neac.DataAccess.Migrations
{
    public partial class update_ProjectFlow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "ProjectFlow",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "ProjectFlow");
        }
    }
}
