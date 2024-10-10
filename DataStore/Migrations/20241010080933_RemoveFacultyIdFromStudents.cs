using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStore.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFacultyIdFromStudents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_students_faculties_faculty_id",
                table: "students");

            migrationBuilder.DropIndex(
                name: "IX_students_faculty_id",
                table: "students");

            migrationBuilder.DropColumn(
                name: "faculty_id",
                table: "students");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "faculty_id",
                table: "students",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_students_faculty_id",
                table: "students",
                column: "faculty_id");

            migrationBuilder.AddForeignKey(
                name: "FK_students_faculties_faculty_id",
                table: "students",
                column: "faculty_id",
                principalTable: "faculties",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
