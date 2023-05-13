using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatRoomTest.Migrations
{
    public partial class AddedMappingDesignTothat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkFromUserId = table.Column<long>(type: "bigint", nullable: false),
                    FkToUserId = table.Column<long>(type: "bigint", nullable: false),
                    MessageContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Like = table.Column<bool>(type: "bit", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Users_FkFromUserId",
                        column: x => x.FkFromUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Messages_Users_FkToUserId",
                        column: x => x.FkToUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserRelations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkUserAId = table.Column<long>(type: "bigint", nullable: false),
                    FkUserBId = table.Column<long>(type: "bigint", nullable: false),
                    Approve = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRelations_Users_FkUserAId",
                        column: x => x.FkUserAId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRelations_Users_FkUserBId",
                        column: x => x.FkUserBId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_FkFromUserId_FkToUserId",
                table: "Messages",
                columns: new[] { "FkFromUserId", "FkToUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_FkToUserId",
                table: "Messages",
                column: "FkToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRelations_FkUserAId_FkUserBId",
                table: "UserRelations",
                columns: new[] { "FkUserAId", "FkUserBId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRelations_FkUserBId",
                table: "UserRelations",
                column: "FkUserBId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "UserRelations");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);
        }
    }
}
