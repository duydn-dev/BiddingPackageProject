using Microsoft.EntityFrameworkCore.Migrations;

namespace Neac.DataAccess.Migrations
{
    public partial class add_Status_and_UserOnline_MeetRooms_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MemberOnline",
                table: "MeetRoom",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "MeetRoom",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MemberOnline",
                table: "MeetRoom");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "MeetRoom");
        }
    }
}
