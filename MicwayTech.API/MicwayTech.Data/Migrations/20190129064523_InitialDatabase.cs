using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace MicwayTech.Data.Migrations
{
    public partial class InitialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DriverDetails",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    firstName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    lastName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    dob = table.Column<DateTime>(nullable: false),
                    email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    isDeleted = table.Column<bool>(nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverDetails", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "DriverDetails",
                columns: new[] { "id", "dob", "email", "firstName", "isDeleted", "lastName" },
                values: new object[,]
                {
                    { "1125e958-4f21-4f2f-98ad-de681a5d7c5b", new DateTime(1900, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "John.Mack@micway.com", "John", false, "Mack" },
                    { "d4f225a1-69f3-4e9a-a626-7dcb33dbc625", new DateTime(1928, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hubert.DAF@micway.com", "Hubert", false, "DAF" },
                    { "3a00db3b-6689-4d4b-9db7-a1d6f641368e", new DateTime(1900, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "George.Kenworth@micway.com", "George", false, "Kenworth" },
                    { "09d4965b-b004-4058-a083-37e72c53dae0", new DateTime(1900, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Benjamin.Caterpillar@micway.com", "Benjamin", false, "Caterpillar" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverDetails");
        }
    }
}
