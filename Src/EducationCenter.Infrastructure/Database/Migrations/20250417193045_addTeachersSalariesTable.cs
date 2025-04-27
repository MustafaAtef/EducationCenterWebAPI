using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationCenterAPI.Database.Migrations
{
    /// <inheritdoc />
    public partial class addTeachersSalariesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentFee_Expenses_ExpenseId",
                table: "StudentFee");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentFee_Students_StudentId",
                table: "StudentFee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentFee",
                table: "StudentFee");

            migrationBuilder.RenameTable(
                name: "StudentFee",
                newName: "StudentFees");

            migrationBuilder.RenameIndex(
                name: "IX_StudentFee_StudentId",
                table: "StudentFees",
                newName: "IX_StudentFees_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentFee_ExpenseId",
                table: "StudentFees",
                newName: "IX_StudentFees_ExpenseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentFees",
                table: "StudentFees",
                columns: new[] { "Months", "StudentId" });

            migrationBuilder.CreateTable(
                name: "TeacherSalaries",
                columns: table => new
                {
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    Months = table.Column<DateOnly>(type: "date", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Paid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpenseId = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherSalaries", x => new { x.Months, x.TeacherId });
                    table.ForeignKey(
                        name: "FK_TeacherSalaries_Expenses_ExpenseId",
                        column: x => x.ExpenseId,
                        principalTable: "Expenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherSalaries_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSalaries_ExpenseId",
                table: "TeacherSalaries",
                column: "ExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherSalaries_TeacherId",
                table: "TeacherSalaries",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentFees_Expenses_ExpenseId",
                table: "StudentFees",
                column: "ExpenseId",
                principalTable: "Expenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentFees_Students_StudentId",
                table: "StudentFees",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentFees_Expenses_ExpenseId",
                table: "StudentFees");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentFees_Students_StudentId",
                table: "StudentFees");

            migrationBuilder.DropTable(
                name: "TeacherSalaries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentFees",
                table: "StudentFees");

            migrationBuilder.RenameTable(
                name: "StudentFees",
                newName: "StudentFee");

            migrationBuilder.RenameIndex(
                name: "IX_StudentFees_StudentId",
                table: "StudentFee",
                newName: "IX_StudentFee_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentFees_ExpenseId",
                table: "StudentFee",
                newName: "IX_StudentFee_ExpenseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentFee",
                table: "StudentFee",
                columns: new[] { "Months", "StudentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_StudentFee_Expenses_ExpenseId",
                table: "StudentFee",
                column: "ExpenseId",
                principalTable: "Expenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentFee_Students_StudentId",
                table: "StudentFee",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
