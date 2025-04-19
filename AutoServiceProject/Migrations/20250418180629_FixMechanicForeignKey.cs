using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoServiceProject.Migrations
{
    /// <inheritdoc />
    public partial class FixMechanicForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequests_Mechanics_MechanicId",
                table: "ServiceRequests");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequests_MechanicId",
                table: "ServiceRequests");

            migrationBuilder.DropColumn(
                name: "MechanicId",
                table: "ServiceRequests");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_SelectedMechanicId",
                table: "ServiceRequests",
                column: "SelectedMechanicId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequests_Mechanics_SelectedMechanicId",
                table: "ServiceRequests",
                column: "SelectedMechanicId",
                principalTable: "Mechanics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequests_Mechanics_SelectedMechanicId",
                table: "ServiceRequests");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequests_SelectedMechanicId",
                table: "ServiceRequests");

            migrationBuilder.AddColumn<int>(
                name: "MechanicId",
                table: "ServiceRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_MechanicId",
                table: "ServiceRequests",
                column: "MechanicId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequests_Mechanics_MechanicId",
                table: "ServiceRequests",
                column: "MechanicId",
                principalTable: "Mechanics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
