using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationCenterAPI.Database.Migrations
{
    /// <inheritdoc />
    public partial class addClassTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GradeId = table.Column<int>(type: "int", nullable: false),
                    SubjectTeacherId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    FromTime = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    Totime = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    AttendanceRegistered = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classes_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Classes_SubjectsTeachers_SubjectTeacherId",
                        column: x => x.SubjectTeacherId,
                        principalTable: "SubjectsTeachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classes_GradeId_Date",
                table: "Classes",
                columns: new[] { "GradeId", "Date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_SubjectTeacherId",
                table: "Classes",
                column: "SubjectTeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Classes");
        }
    }
}
