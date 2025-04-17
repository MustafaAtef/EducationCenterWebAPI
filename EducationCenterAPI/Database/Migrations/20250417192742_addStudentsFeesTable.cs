using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationCenterAPI.Database.Migrations
{
    /// <inheritdoc />
    public partial class addStudentsFeesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentFee",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    Months = table.Column<DateOnly>(type: "date", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Paid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpenseId = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentFee", x => new { x.Months, x.StudentId });
                    table.ForeignKey(
                        name: "FK_StudentFee_Expenses_ExpenseId",
                        column: x => x.ExpenseId,
                        principalTable: "Expenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentFee_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentFee_ExpenseId",
                table: "StudentFee",
                column: "ExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentFee_StudentId",
                table: "StudentFee",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentFee");
        }
    }
}
