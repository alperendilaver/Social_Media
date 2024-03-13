using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApp.Migrations.Blog
{
    /// <inheritdoc />
    public partial class ReactionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMemberShipRequests_AppUser_UserId",
                table: "GroupMemberShipRequests");

            migrationBuilder.DropColumn(
                name: "reaction",
                table: "Posts");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "GroupMemberShipRequests",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "reactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    PostId = table.Column<int>(type: "INTEGER", nullable: false),
                    reaction = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_reactions_AppUser_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reactions_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_reactions_PostId",
                table: "reactions",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_reactions_UserId",
                table: "reactions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMemberShipRequests_AppUser_UserId",
                table: "GroupMemberShipRequests",
                column: "UserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMemberShipRequests_AppUser_UserId",
                table: "GroupMemberShipRequests");

            migrationBuilder.DropTable(
                name: "reactions");

            migrationBuilder.AddColumn<int>(
                name: "reaction",
                table: "Posts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "GroupMemberShipRequests",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMemberShipRequests_AppUser_UserId",
                table: "GroupMemberShipRequests",
                column: "UserId",
                principalTable: "AppUser",
                principalColumn: "Id");
        }
    }
}
