using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OdevDagitimPortali.Migrations
{
    /// <inheritdoc />
    public partial class mig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fullname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    user_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    assignment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dead_line = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    user_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.assignment_id);
                    table.ForeignKey(
                        name: "FK_Assignments_User_user_ID",
                        column: x => x.user_ID,
                        principalTable: "User",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Submissions",
                columns: table => new
                {
                    submission_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    assignment_ID = table.Column<int>(type: "int", nullable: false),
                    user_ID = table.Column<int>(type: "int", nullable: false),
                    submission_date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    file_url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissions", x => x.submission_id);
                    table.ForeignKey(
                        name: "FK_Submissions_Assignments_assignment_ID",
                        column: x => x.assignment_ID,
                        principalTable: "Assignments",
                        principalColumn: "assignment_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Submissions_User_user_ID",
                        column: x => x.user_ID,
                        principalTable: "User",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_user_ID",
                table: "Assignments",
                column: "user_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_assignment_ID",
                table: "Submissions",
                column: "assignment_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_user_ID",
                table: "Submissions",
                column: "user_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Submissions");

            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropTable(
                name: "User");


            migrationBuilder.DropForeignKey(
            name: "FK_Submissions_User_user_ID",
            table: "Submissions");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_User_user_ID",
                table: "Submissions",
                column: "user_ID",
                principalTable: "Users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade); // Eski cascade davranışını geri al
        }
    }
}
