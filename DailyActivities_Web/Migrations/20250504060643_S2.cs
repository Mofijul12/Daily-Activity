using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailyActivities_Web.Migrations
{
    /// <inheritdoc />
    public partial class S2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyActivities_AspNetUsers_UserId",
                table: "DailyActivities");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "DailyActivities",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "SleepTime",
                table: "DailyActivities",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "GetUpTime",
                table: "DailyActivities",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyActivities_AspNetUsers_UserId",
                table: "DailyActivities",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyActivities_AspNetUsers_UserId",
                table: "DailyActivities");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "DailyActivities",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "SleepTime",
                table: "DailyActivities",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "GetUpTime",
                table: "DailyActivities",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DailyActivities_AspNetUsers_UserId",
                table: "DailyActivities",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
