using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApplication1.Data.Migrations
{
    /// <inheritdoc />
    public partial class fileshare : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "FileData",
                table: "Messages",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileType",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileData",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "FileType",
                table: "Messages");
        }
    }
}
