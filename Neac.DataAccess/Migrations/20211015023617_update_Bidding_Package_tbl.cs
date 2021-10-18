using Microsoft.EntityFrameworkCore.Migrations;

namespace Neac.DataAccess.Migrations
{
    public partial class update_Bidding_Package_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "BiddingPackage");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "BiddingPackageProject",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "BiddingPackageProject");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "BiddingPackage",
                type: "int",
                nullable: true);
        }
    }
}
