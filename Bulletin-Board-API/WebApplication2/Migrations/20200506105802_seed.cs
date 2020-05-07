using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication2.Migrations
{
    public partial class seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Brands",
                keyColumn: "BrandId",
                keyValue: 77,
                column: "Title",
                value: "Ekomia");

            migrationBuilder.UpdateData(
                table: "Brands",
                keyColumn: "BrandId",
                keyValue: 99,
                column: "Title",
                value: "Yamaha");

            migrationBuilder.UpdateData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 1,
                columns: new[] { "CoordX", "CoordY", "Title" },
                values: new object[] { 250, 300, "Balti" });

            migrationBuilder.UpdateData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 2,
                columns: new[] { "CoordX", "CoordY", "Title" },
                values: new object[] { 100, 115, "Briceni" });

            migrationBuilder.InsertData(
                table: "Towns",
                columns: new[] { "TownId", "CoordX", "CoordY", "Title" },
                values: new object[,]
                {
                    { 32, 100, 115, "Vulcanesti" },
                    { 31, 100, 115, "Vatra" },
                    { 30, 100, 115, "Ungheni" },
                    { 29, 100, 115, "Tiraspol" },
                    { 28, 100, 115, "Tighina" },
                    { 27, 100, 115, "Taraclia" },
                    { 26, 100, 115, "Straseni" },
                    { 25, 100, 115, "Soroca" },
                    { 24, 100, 115, "Riscani" },
                    { 23, 100, 115, "Rezina" },
                    { 22, 100, 115, "Ocnita" },
                    { 21, 100, 115, "Orhei" },
                    { 20, 100, 115, "Nisporeni" },
                    { 18, 100, 115, "Leova" },
                    { 17, 100, 115, "Ialoveni" },
                    { 16, 100, 115, "Hincesti" },
                    { 15, 100, 115, "Glodeni" },
                    { 14, 100, 115, "Falesti" },
                    { 13, 100, 115, "Edinet" },
                    { 12, 100, 115, "Durlesti" },
                    { 11, 100, 115, "Dubasar" },
                    { 10, 100, 115, "Drochia" },
                    { 9, 199, 250, "Chisinau" },
                    { 8, 100, 115, "Cricova" },
                    { 7, 100, 115, "Comrat" },
                    { 6, 100, 115, "Calarasi" },
                    { 5, 100, 115, "Camtemir" },
                    { 19, 100, 115, "Lipcani" },
                    { 4, 100, 115, "Camenca" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Towns_Title",
                table: "Towns",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Title",
                table: "Categories",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Brands_Title",
                table: "Brands",
                column: "Title",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Towns_Title",
                table: "Towns");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Title",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Brands_Title",
                table: "Brands");

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 32);

            migrationBuilder.UpdateData(
                table: "Brands",
                keyColumn: "BrandId",
                keyValue: 77,
                column: "Title",
                value: "Zara");

            migrationBuilder.UpdateData(
                table: "Brands",
                keyColumn: "BrandId",
                keyValue: 99,
                column: "Title",
                value: "Toshiba");

            migrationBuilder.UpdateData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 1,
                columns: new[] { "CoordX", "CoordY", "Title" },
                values: new object[] { 199, 250, "Chisinau" });

            migrationBuilder.UpdateData(
                table: "Towns",
                keyColumn: "TownId",
                keyValue: 2,
                columns: new[] { "CoordX", "CoordY", "Title" },
                values: new object[] { 250, 300, "Balti" });
        }
    }
}
