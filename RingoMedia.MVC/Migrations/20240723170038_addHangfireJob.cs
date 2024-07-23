using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RingoMedia.MVC.Migrations
{
    /// <inheritdoc />
    public partial class addHangfireJob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HangfireJobId",
                table: "EmailsLog",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HangfireJobId",
                table: "EmailsLog");
        }
    }
}
