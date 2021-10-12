using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Neac.DataAccess.Migrations
{
    public partial class add_3_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GroupRoleId",
                table: "Roles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BiddingPackage",
                columns: table => new
                {
                    BiddingPackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BiddingPackageName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BiddingPackage", x => x.BiddingPackageId);
                });

            migrationBuilder.CreateTable(
                name: "GroupRole",
                columns: table => new
                {
                    GroupRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupRoleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupRoleCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupRole", x => x.GroupRoleId);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentState = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.ProjectId);
                });

            migrationBuilder.CreateTable(
                name: "BiddingPackageProject",
                columns: table => new
                {
                    BiddingPackageProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BiddingPackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BiddingPackageProject", x => x.BiddingPackageProjectId);
                    table.ForeignKey(
                        name: "FK_BiddingPackageProject_BiddingPackage_BiddingPackageId",
                        column: x => x.BiddingPackageId,
                        principalTable: "BiddingPackage",
                        principalColumn: "BiddingPackageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BiddingPackageProject_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Roles_GroupRoleId",
                table: "Roles",
                column: "GroupRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_BiddingPackageProject_BiddingPackageId",
                table: "BiddingPackageProject",
                column: "BiddingPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_BiddingPackageProject_ProjectId",
                table: "BiddingPackageProject",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_GroupRole_GroupRoleId",
                table: "Roles",
                column: "GroupRoleId",
                principalTable: "GroupRole",
                principalColumn: "GroupRoleId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_GroupRole_GroupRoleId",
                table: "Roles");

            migrationBuilder.DropTable(
                name: "BiddingPackageProject");

            migrationBuilder.DropTable(
                name: "GroupRole");

            migrationBuilder.DropTable(
                name: "BiddingPackage");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Roles_GroupRoleId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "GroupRoleId",
                table: "Roles");
        }
    }
}
