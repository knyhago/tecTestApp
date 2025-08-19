using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PerformedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Forename = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DateOfBirth", "Email", "Forename", "IsActive", "Surname" },
                values: new object[,]
                {
                    { 1L, new DateOnly(1995, 6, 12), "ploew@example.com", "Peter", true, "Loew" },
                    { 2L, new DateOnly(1995, 11, 21), "bfgates@example.com", "Benjamin Franklin", true, "Gates" },
                    { 3L, new DateOnly(1998, 4, 15), "ctroy@example.com", "Castor", false, "Troy" },
                    { 4L, new DateOnly(2005, 3, 17), "mraines@example.com", "Memphis", true, "Raines" },
                    { 5L, new DateOnly(1980, 5, 20), "sgodspeed@example.com", "Stanley", true, "Goodspeed" },
                    { 6L, new DateOnly(2000, 12, 18), "himcdunnough@example.com", "H.I.", true, "McDunnough" },
                    { 7L, new DateOnly(2002, 2, 27), "cpoe@example.com", "Cameron", false, "Poe" },
                    { 8L, new DateOnly(2012, 12, 13), "emalus@example.com", "Edward", false, "Malus" },
                    { 9L, new DateOnly(2008, 12, 29), "dmacready@example.com", "Damon", false, "Macready" },
                    { 10L, new DateOnly(1999, 2, 17), "jblaze@example.com", "Johnny", true, "Blaze" },
                    { 11L, new DateOnly(1997, 1, 6), "rfeld@example.com", "Robin", true, "Feld" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLogs");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
