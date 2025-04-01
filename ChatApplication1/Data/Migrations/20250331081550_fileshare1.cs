using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApplication1.Data.Migrations
{
    /// <inheritdoc />
    public partial class fileshare1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "Messages",
                newName: "FileName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Messages",
                newName: "FilePath");
        }
    }
}
