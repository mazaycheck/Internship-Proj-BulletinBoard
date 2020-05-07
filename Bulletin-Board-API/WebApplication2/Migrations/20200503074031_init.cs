using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication2.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    BrandId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.BrandId);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Towns",
                columns: table => new
                {
                    TownId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 30, nullable: false),
                    CoordX = table.Column<int>(nullable: false),
                    CoordY = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Towns", x => x.TownId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BrandCategories",
                columns: table => new
                {
                    BrandCategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandCategories", x => x.BrandCategoryId);
                    table.UniqueConstraint("AK_BrandCategories_BrandId_CategoryId", x => new { x.BrandId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_BrandCategories_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    TownId = table.Column<int>(nullable: true),
                    RegistrationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Towns_TownId",
                        column: x => x.TownId,
                        principalTable: "Towns",
                        principalColumn: "TownId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Annoucements",
                columns: table => new
                {
                    AnnoucementId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 2000, nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    ExpirationDate = table.Column<DateTime>(nullable: true),
                    Price = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    BrandCategoryId = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    Sex = table.Column<bool>(nullable: true),
                    Size = table.Column<int>(nullable: true),
                    Color = table.Column<string>(nullable: true),
                    ScreenSize = table.Column<double>(nullable: true),
                    Processor = table.Column<string>(nullable: true),
                    Ram = table.Column<int>(nullable: true),
                    VehicleAnnoucementId = table.Column<int>(nullable: true),
                    EngineType = table.Column<string>(nullable: true),
                    Mileage = table.Column<int>(nullable: true),
                    VehicleAnnoucement_Color = table.Column<string>(nullable: true),
                    Year = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Annoucements", x => x.AnnoucementId);
                    table.ForeignKey(
                        name: "FK_Annoucements_BrandCategories_BrandCategoryId",
                        column: x => x.BrandCategoryId,
                        principalTable: "BrandCategories",
                        principalColumn: "BrandCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Annoucements_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(nullable: true),
                    SenderId = table.Column<int>(nullable: false),
                    RecieverId = table.Column<int>(nullable: false),
                    DateTimeSent = table.Column<DateTime>(nullable: true),
                    DateTimeRead = table.Column<DateTime>(nullable: true),
                    IsRead = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_RecieverId",
                        column: x => x.RecieverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    SubscriptionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.SubscriptionId);
                    table.ForeignKey(
                        name: "FK_Subscriptions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfiles_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    PhotoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhotoUrl = table.Column<string>(nullable: true),
                    AnnoucementId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.PhotoId);
                    table.ForeignKey(
                        name: "FK_Photos_Annoucements_AnnoucementId",
                        column: x => x.AnnoucementId,
                        principalTable: "Annoucements",
                        principalColumn: "AnnoucementId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "BrandId", "Title" },
                values: new object[,]
                {
                    { 1, "Other" },
                    { 75, "Hay" },
                    { 74, "Knoll" },
                    { 73, "Arper" },
                    { 72, "Ikea" },
                    { 71, "Vitra" },
                    { 70, "ROCA " },
                    { 69, "Citizen" },
                    { 68, "Longines" },
                    { 67, "Festina" },
                    { 66, "Swatch" },
                    { 65, "Fossil" },
                    { 64, "Timex" },
                    { 63, "Orient" },
                    { 62, "Casio" },
                    { 61, "Seiko" },
                    { 59, "Fujitsu" },
                    { 58, "Panasonic" },
                    { 57, "Razer" },
                    { 56, "Msi" },
                    { 76, "Moroso" },
                    { 77, "Zara" },
                    { 78, "B&B Italia" },
                    { 79, "Minotti" },
                    { 99, "Toshiba" },
                    { 98, "Logic" },
                    { 97, "Nikon" },
                    { 96, "Epson" },
                    { 95, "Vesta" },
                    { 94, "Sigma" },
                    { 93, "Jbl" },
                    { 92, "Benq" },
                    { 91, "Canon" },
                    { 55, "Microsoft" },
                    { 90, "GoPro" },
                    { 88, "Rowenta" },
                    { 87, "Atlant" },
                    { 86, "Bosch" },
                    { 85, "Gefest" },
                    { 84, "Gorenje" },
                    { 83, "Philips" },
                    { 82, "Vitek" },
                    { 81, "Whirlpool" },
                    { 80, "Electrolux" },
                    { 89, "Kaiser" },
                    { 54, "Dell" },
                    { 60, "Omega" },
                    { 52, "Sony" },
                    { 28, "Google Pixel" },
                    { 27, "LG" },
                    { 26, "Nokia" },
                    { 53, "Acer" },
                    { 24, "Motorola" },
                    { 23, "Xiaomi" },
                    { 22, "Huawei" },
                    { 21, "Apple" },
                    { 20, "Samsung" },
                    { 29, "Asus" },
                    { 19, "Alfa-Romeo" },
                    { 17, "Mercedez-Benz" },
                    { 16, "Nissan" },
                    { 15, "BMW" },
                    { 14, "Fiat" },
                    { 13, "Honda" },
                    { 12, "Ford" },
                    { 11, "Volkswagen" },
                    { 10, "Toyota" },
                    { 2, "None" },
                    { 18, "Acura" },
                    { 30, "Under Armour" },
                    { 25, "Lenovo" },
                    { 32, "Adidas" },
                    { 51, "Hewlett-Packard" },
                    { 50, "Toshiba" },
                    { 49, "Armani" },
                    { 48, "The North Face" },
                    { 31, "Reebok" },
                    { 46, "Hugo Boss" },
                    { 45, "Versace" },
                    { 44, "Levi Strauss" },
                    { 43, "Zara" },
                    { 47, "Lacoste" },
                    { 41, "Tommy Hilfiger" },
                    { 42, "Ralph Lauren" },
                    { 34, "Converse" },
                    { 35, "Puma" },
                    { 36, "New Balance" },
                    { 33, "Nike" },
                    { 38, "Geox" },
                    { 39, "Vans" },
                    { 40, "ASOS" },
                    { 37, "Fila" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Title" },
                values: new object[,]
                {
                    { 9, "Audio/Video" },
                    { 1, "Vehicles" },
                    { 2, "Mobile Phones" },
                    { 3, "Shoes" },
                    { 4, "Clothes" },
                    { 5, "Computers" },
                    { 6, "Watches" },
                    { 7, "Furniture" },
                    { 8, "Appliances" },
                    { 10, "Services" }
                });

            migrationBuilder.InsertData(
                table: "Towns",
                columns: new[] { "TownId", "CoordX", "CoordY", "Title" },
                values: new object[,]
                {
                    { 2, 250, 300, "Balti" },
                    { 1, 199, 250, "Chisinau" },
                    { 3, 100, 115, "Cahul" }
                });

            migrationBuilder.InsertData(
                table: "BrandCategories",
                columns: new[] { "BrandCategoryId", "BrandId", "CategoryId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 705, 75, 7 },
                    { 704, 74, 7 },
                    { 703, 73, 7 },
                    { 702, 72, 7 },
                    { 701, 71, 7 },
                    { 700, 70, 7 },
                    { 7, 1, 7 },
                    { 609, 69, 6 },
                    { 608, 68, 6 },
                    { 607, 67, 6 },
                    { 606, 66, 6 },
                    { 605, 65, 6 },
                    { 604, 64, 6 },
                    { 603, 63, 6 },
                    { 602, 62, 6 },
                    { 601, 61, 6 },
                    { 600, 60, 6 },
                    { 6, 1, 6 },
                    { 509, 59, 5 },
                    { 508, 58, 5 },
                    { 507, 57, 5 },
                    { 706, 76, 7 },
                    { 506, 56, 5 },
                    { 707, 77, 7 },
                    { 709, 79, 7 },
                    { 908, 98, 9 },
                    { 907, 97, 9 },
                    { 906, 96, 9 },
                    { 905, 95, 9 },
                    { 904, 94, 9 },
                    { 903, 93, 9 },
                    { 902, 92, 9 },
                    { 901, 91, 9 },
                    { 900, 90, 9 },
                    { 9, 1, 9 },
                    { 809, 89, 8 },
                    { 808, 88, 8 },
                    { 807, 87, 8 },
                    { 806, 86, 8 },
                    { 805, 85, 8 },
                    { 804, 84, 8 },
                    { 803, 83, 8 },
                    { 802, 82, 8 },
                    { 801, 81, 8 },
                    { 800, 80, 8 },
                    { 8, 1, 8 },
                    { 708, 78, 7 },
                    { 505, 55, 5 },
                    { 504, 54, 5 },
                    { 503, 53, 5 },
                    { 209, 29, 2 },
                    { 208, 28, 2 },
                    { 207, 27, 2 },
                    { 206, 26, 2 },
                    { 205, 25, 2 },
                    { 204, 24, 2 },
                    { 203, 23, 2 },
                    { 202, 22, 2 },
                    { 201, 21, 2 },
                    { 200, 20, 2 },
                    { 2, 1, 2 },
                    { 109, 19, 1 },
                    { 108, 18, 1 },
                    { 107, 17, 1 },
                    { 106, 16, 1 },
                    { 105, 15, 1 },
                    { 104, 14, 1 },
                    { 103, 13, 1 },
                    { 102, 12, 1 },
                    { 101, 11, 1 },
                    { 100, 10, 1 },
                    { 3, 1, 3 },
                    { 300, 30, 3 },
                    { 301, 31, 3 },
                    { 302, 32, 3 },
                    { 502, 52, 5 },
                    { 501, 51, 5 },
                    { 500, 50, 5 },
                    { 5, 1, 5 },
                    { 409, 49, 4 },
                    { 408, 48, 4 },
                    { 407, 47, 4 },
                    { 406, 46, 4 },
                    { 405, 45, 4 },
                    { 404, 44, 4 },
                    { 909, 99, 9 },
                    { 403, 43, 4 },
                    { 401, 41, 4 },
                    { 400, 40, 4 },
                    { 4, 1, 4 },
                    { 309, 39, 3 },
                    { 308, 38, 3 },
                    { 307, 37, 3 },
                    { 306, 36, 3 },
                    { 305, 35, 3 },
                    { 304, 34, 3 },
                    { 303, 33, 3 },
                    { 402, 42, 4 },
                    { 10, 2, 10 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Annoucements_BrandCategoryId",
                table: "Annoucements",
                column: "BrandCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Annoucements_UserId",
                table: "Annoucements",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TownId",
                table: "AspNetUsers",
                column: "TownId");

            migrationBuilder.CreateIndex(
                name: "IX_BrandCategories_CategoryId",
                table: "BrandCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_RecieverId",
                table: "Messages",
                column: "RecieverId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_AnnoucementId",
                table: "Photos",
                column: "AnnoucementId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_UserId",
                table: "Subscriptions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Annoucements");

            migrationBuilder.DropTable(
                name: "BrandCategories");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Towns");
        }
    }
}
