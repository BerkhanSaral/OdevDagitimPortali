using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OdevDagitimPortali.Migrations
{
    /// <inheritdoc />
    public partial class mig6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileUpload_User_user_ID",
                table: "FileUpload");

            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_FileUpload_file_urlId",
                table: "Submissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FileUpload",
                table: "FileUpload");

            migrationBuilder.RenameTable(
                name: "FileUpload",
                newName: "FileUploads");

            migrationBuilder.RenameColumn(
                name: "file_urlId",
                table: "Submissions",
                newName: "FileUploadId");

            migrationBuilder.RenameIndex(
                name: "IX_Submissions_file_urlId",
                table: "Submissions",
                newName: "IX_Submissions_FileUploadId");

            migrationBuilder.RenameIndex(
                name: "IX_FileUpload_user_ID",
                table: "FileUploads",
                newName: "IX_FileUploads_user_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileUploads",
                table: "FileUploads",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FileUploads_User_user_ID",
                table: "FileUploads",
                column: "user_ID",
                principalTable: "User",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_FileUploads_FileUploadId",
                table: "Submissions",
                column: "FileUploadId",
                principalTable: "FileUploads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileUploads_User_user_ID",
                table: "FileUploads");

            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_FileUploads_FileUploadId",
                table: "Submissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FileUploads",
                table: "FileUploads");

            migrationBuilder.RenameTable(
                name: "FileUploads",
                newName: "FileUpload");

            migrationBuilder.RenameColumn(
                name: "FileUploadId",
                table: "Submissions",
                newName: "file_urlId");

            migrationBuilder.RenameIndex(
                name: "IX_Submissions_FileUploadId",
                table: "Submissions",
                newName: "IX_Submissions_file_urlId");

            migrationBuilder.RenameIndex(
                name: "IX_FileUploads_user_ID",
                table: "FileUpload",
                newName: "IX_FileUpload_user_ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileUpload",
                table: "FileUpload",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FileUpload_User_user_ID",
                table: "FileUpload",
                column: "user_ID",
                principalTable: "User",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_FileUpload_file_urlId",
                table: "Submissions",
                column: "file_urlId",
                principalTable: "FileUpload",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
