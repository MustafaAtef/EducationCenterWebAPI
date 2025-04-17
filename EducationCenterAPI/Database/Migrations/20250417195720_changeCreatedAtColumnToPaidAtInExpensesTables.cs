using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationCenterAPI.Database.Migrations
{
    /// <inheritdoc />
    public partial class changeCreatedAtColumnToPaidAtInExpensesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "TeacherSalaries",
                newName: "PaidAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "StudentFees",
                newName: "PaidAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "OtherExpenses",
                newName: "PaidAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Expenses",
                newName: "PaidAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaidAt",
                table: "TeacherSalaries",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "PaidAt",
                table: "StudentFees",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "PaidAt",
                table: "OtherExpenses",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "PaidAt",
                table: "Expenses",
                newName: "CreatedAt");
        }
    }
}
