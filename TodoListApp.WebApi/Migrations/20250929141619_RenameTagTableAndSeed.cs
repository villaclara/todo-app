using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoListApp.WebApi.Migrations
{
    public partial class RenameTagTableAndSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoTaskEntityTodoTaskTagEntity_TodoTaskTagEntity_TagListId",
                table: "TodoTaskEntityTodoTaskTagEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TodoTaskTagEntity",
                table: "TodoTaskTagEntity");

            migrationBuilder.RenameTable(
                name: "TodoTaskTagEntity",
                newName: "TodoTaskTags");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TodoTaskTags",
                table: "TodoTaskTags",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoTaskEntityTodoTaskTagEntity_TodoTaskTags_TagListId",
                table: "TodoTaskEntityTodoTaskTagEntity",
                column: "TagListId",
                principalTable: "TodoTaskTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoTaskEntityTodoTaskTagEntity_TodoTaskTags_TagListId",
                table: "TodoTaskEntityTodoTaskTagEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TodoTaskTags",
                table: "TodoTaskTags");

            migrationBuilder.RenameTable(
                name: "TodoTaskTags",
                newName: "TodoTaskTagEntity");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TodoTaskTagEntity",
                table: "TodoTaskTagEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoTaskEntityTodoTaskTagEntity_TodoTaskTagEntity_TagListId",
                table: "TodoTaskEntityTodoTaskTagEntity",
                column: "TagListId",
                principalTable: "TodoTaskTagEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
