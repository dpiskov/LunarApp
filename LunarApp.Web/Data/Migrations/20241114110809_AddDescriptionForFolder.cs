using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LunarApp.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionForFolder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Folders",
                type: "nvarchar(max)",
                maxLength: 20000,
                nullable: true,
                comment: "Folder description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Folders");
        }
    }
}
