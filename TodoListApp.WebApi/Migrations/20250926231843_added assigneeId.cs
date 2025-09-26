using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoListApp.WebApi.Migrations
{
    public partial class addedassigneeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Assignee",
                table: "TodoTasks",
                newName: "AssigneeName");

            migrationBuilder.AddColumn<int>(
                name: "AssigneeId",
                table: "TodoTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssigneeId",
                table: "TodoTasks");

            migrationBuilder.RenameColumn(
                name: "AssigneeName",
                table: "TodoTasks",
                newName: "Assignee");
        }
    }
}
