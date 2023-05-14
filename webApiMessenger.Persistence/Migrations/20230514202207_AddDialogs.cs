using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webApiMessenger.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDialogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "GroupChats",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "GroupChats");
        }
    }
}
