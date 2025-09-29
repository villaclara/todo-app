using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoListApp.WebApi.Migrations
{
    public partial class addedTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TodoTaskTagEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoTaskTagEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TodoTaskEntityTodoTaskTagEntity",
                columns: table => new
                {
                    TagListId = table.Column<int>(type: "int", nullable: false),
                    TodoTasksId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoTaskEntityTodoTaskTagEntity", x => new { x.TagListId, x.TodoTasksId });
                    table.ForeignKey(
                        name: "FK_TodoTaskEntityTodoTaskTagEntity_TodoTasks_TodoTasksId",
                        column: x => x.TodoTasksId,
                        principalTable: "TodoTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TodoTaskEntityTodoTaskTagEntity_TodoTaskTagEntity_TagListId",
                        column: x => x.TagListId,
                        principalTable: "TodoTaskTagEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TodoTaskTagEntity",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 1, "Work" },
                    { 2, "Personal" },
                    { 3, "Urgent" },
                    { 4, "Low Priority" },
                    { 5, "Errands" },
                    { 6, "Shopping" },
                    { 7, "Health" },
                    { 8, "Finance" },
                    { 9, "Study" },
                    { 10, "Travel" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TodoTaskEntityTodoTaskTagEntity_TodoTasksId",
                table: "TodoTaskEntityTodoTaskTagEntity",
                column: "TodoTasksId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TodoTaskEntityTodoTaskTagEntity");

            migrationBuilder.DropTable(
                name: "TodoTaskTagEntity");
        }
    }
}
