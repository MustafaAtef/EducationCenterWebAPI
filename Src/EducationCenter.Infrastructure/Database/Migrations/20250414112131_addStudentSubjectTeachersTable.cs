using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationCenterAPI.Database.Migrations
{
    /// <inheritdoc />
    public partial class addStudentSubjectTeachersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentSubjectsTeachers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    SubjectTeacherId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentSubjectsTeachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentSubjectsTeachers_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentSubjectsTeachers_SubjectsTeachers_SubjectTeacherId",
                        column: x => x.SubjectTeacherId,
                        principalTable: "SubjectsTeachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentSubjectsTeachers_StudentId",
                table: "StudentSubjectsTeachers",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentSubjectsTeachers_SubjectTeacherId",
                table: "StudentSubjectsTeachers",
                column: "SubjectTeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentSubjectsTeachers");
        }
    }
}
