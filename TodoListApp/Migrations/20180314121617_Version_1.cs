using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TodoListApp.Migrations
{
    public partial class Version_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    UserName = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.UserName);
                });

            migrationBuilder.CreateTable(
                name: "TodoList",
                columns: table => new
                {
                    UserName = table.Column<string>(nullable: false),
                    What = table.Column<string>(nullable: false),
                    ActualDate = table.Column<DateTime>(nullable: false),
                    DeadLine = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoList", x => new { x.UserName, x.What });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "TodoList");
        }
    }
}
