using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreBoxBackend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToModuleEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Modules",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Modules");
        }
    }
}
