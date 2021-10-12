using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Neac.DataAccess.Migrations
{
    public partial class add_User_Position_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserPositionId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserPosition",
                columns: table => new
                {
                    UserPositionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserPositionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPosition", x => x.UserPositionId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserPositionId",
                table: "Users",
                column: "UserPositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserPosition_UserPositionId",
                table: "Users",
                column: "UserPositionId",
                principalTable: "UserPosition",
                principalColumn: "UserPositionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserPosition_UserPositionId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "UserPosition");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserPositionId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserPositionId",
                table: "Users");
        }
    }
}
