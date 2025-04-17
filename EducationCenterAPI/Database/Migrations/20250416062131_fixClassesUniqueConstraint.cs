using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationCenterAPI.Database.Migrations
{
    /// <inheritdoc />
    public partial class fixClassesUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Classes_GradeId_Date",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_SubjectTeacherId",
                table: "Classes");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_GradeId",
                table: "Classes",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_SubjectTeacherId_Date",
                table: "Classes",
                columns: new[] { "SubjectTeacherId", "Date" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Classes_GradeId",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_SubjectTeacherId_Date",
                table: "Classes");

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
    }
}
