using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationCenterAPI.Database.Migrations
{
    /// <inheritdoc />
    public partial class addStudentIdSubjectTeacherIdUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentSubjectsTeachers_StudentId",
                table: "StudentSubjectsTeachers");

            migrationBuilder.CreateIndex(
                name: "IX_StudentSubjectsTeachers_StudentId_SubjectTeacherId",
                table: "StudentSubjectsTeachers",
                columns: new[] { "StudentId", "SubjectTeacherId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentSubjectsTeachers_StudentId_SubjectTeacherId",
                table: "StudentSubjectsTeachers");

            migrationBuilder.CreateIndex(
                name: "IX_StudentSubjectsTeachers_StudentId",
                table: "StudentSubjectsTeachers",
                column: "StudentId");
        }
    }
}
