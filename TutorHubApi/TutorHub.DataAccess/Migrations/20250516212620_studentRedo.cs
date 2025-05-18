using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorHub.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class studentRedo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Grage",
                table: "Students",
                newName: "Grade");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Grade",
                table: "Students",
                newName: "Grage");
        }
    }
}
