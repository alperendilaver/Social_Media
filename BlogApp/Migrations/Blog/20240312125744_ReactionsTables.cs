using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApp.Migrations.Blog
{
    /// <inheritdoc />
    public partial class ReactionsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_reactions_PostId",
                table: "reactions");

            migrationBuilder.CreateIndex(
                name: "IX_reactions_PostId",
                table: "reactions",
                column: "PostId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_reactions_PostId",
                table: "reactions");

            migrationBuilder.CreateIndex(
                name: "IX_reactions_PostId",
                table: "reactions",
                column: "PostId");
        }
    }
}
