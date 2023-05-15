using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatRoomTest.Migrations
{
    public partial class AddTimeOfmessageToMessageModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Messages_FkFromUserId_FkToUserId",
                table: "Messages");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TimeOffset",
                table: "Messages",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 5, 15, 18, 54, 10, 898, DateTimeKind.Unspecified).AddTicks(709), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateIndex(
                name: "IX_Messages_FkFromUserId",
                table: "Messages",
                column: "FkFromUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Messages_FkFromUserId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "TimeOffset",
                table: "Messages");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_FkFromUserId_FkToUserId",
                table: "Messages",
                columns: new[] { "FkFromUserId", "FkToUserId" });
        }
    }
}
