using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QueryR.EntityFrameworkCore.Tests.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Age = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PetTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    OwnerId = table.Column<int>(type: "INTEGER", nullable: false),
                    PetTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pets_Persons_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pets_PetTypes_PetTypeId",
                        column: x => x.PetTypeId,
                        principalTable: "PetTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Age", "Name" },
                values: new object[] { 1, 0, "Craig" });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Age", "Name" },
                values: new object[] { 2, 0, "Also Craig" });

            migrationBuilder.InsertData(
                table: "PetTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Cat" });

            migrationBuilder.InsertData(
                table: "PetTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Dog" });

            migrationBuilder.InsertData(
                table: "Pets",
                columns: new[] { "Id", "Name", "OwnerId", "PetTypeId" },
                values: new object[] { 1, "Titan", 1, 2 });

            migrationBuilder.InsertData(
                table: "Pets",
                columns: new[] { "Id", "Name", "OwnerId", "PetTypeId" },
                values: new object[] { 2, "Rufus", 2, 2 });

            migrationBuilder.InsertData(
                table: "Pets",
                columns: new[] { "Id", "Name", "OwnerId", "PetTypeId" },
                values: new object[] { 3, "Meowswers", 1, 1 });

            migrationBuilder.InsertData(
                table: "Pets",
                columns: new[] { "Id", "Name", "OwnerId", "PetTypeId" },
                values: new object[] { 4, "Kitty", 2, 2 });

            migrationBuilder.CreateIndex(
                name: "IX_Pets_OwnerId",
                table: "Pets",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_PetTypeId",
                table: "Pets",
                column: "PetTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pets");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "PetTypes");
        }
    }
}
