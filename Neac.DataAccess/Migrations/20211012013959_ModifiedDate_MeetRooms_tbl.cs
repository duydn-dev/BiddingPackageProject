using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Neac.DataAccess.Migrations
{
    public partial class ModifiedDate_MeetRooms_tbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "MeetRoom",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "MeetRoom");
        }
    }
}
