using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OdevDagitimPortali.Migrations
{
    /// <inheritdoc />
    public partial class mig5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "file_url",
                table: "Submissions");

            migrationBuilder.AddColumn<int>(
                name: "file_urlId",
                table: "Submissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FileUpload",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileContent = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    user_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileUpload", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileUpload_User_user_ID",
                        column: x => x.user_ID,
                        principalTable: "User",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_file_urlId",
                table: "Submissions",
                column: "file_urlId");

            migrationBuilder.CreateIndex(
                name: "IX_FileUpload_user_ID",
                table: "FileUpload",
                column: "user_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_FileUpload_file_urlId",
                table: "Submissions",
                column: "file_urlId",
                principalTable: "FileUpload",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_FileUpload_file_urlId",
                table: "Submissions");

            migrationBuilder.DropTable(
                name: "FileUpload");

            migrationBuilder.DropIndex(
                name: "IX_Submissions_file_urlId",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "file_urlId",
                table: "Submissions");

            migrationBuilder.AddColumn<string>(
                name: "file_url",
                table: "Submissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
