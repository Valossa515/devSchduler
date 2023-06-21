using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevScheduler.API.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DevSchedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Start_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevSchedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "devScheduleSpeakers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TalkTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TalkDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkedInProfile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DevScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_devScheduleSpeakers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_devScheduleSpeakers_DevSchedules_DevScheduleId",
                        column: x => x.DevScheduleId,
                        principalTable: "DevSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_devScheduleSpeakers_DevScheduleId",
                table: "devScheduleSpeakers",
                column: "DevScheduleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "devScheduleSpeakers");

            migrationBuilder.DropTable(
                name: "DevSchedules");
        }
    }
}
