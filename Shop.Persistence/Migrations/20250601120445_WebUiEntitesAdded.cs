using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class WebUiEntitesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DisCountAreas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisCountAreas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomeSliderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BackgroundImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeSliderItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TopCategoryAreas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopCategoryAreas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopCategoryAreas_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DisCountAreaLanguages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LangCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisCountAreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisCountAreaLanguages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DisCountAreaLanguages_DisCountAreas_DisCountAreaId",
                        column: x => x.DisCountAreaId,
                        principalTable: "DisCountAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HomeSliderLanguages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LangCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomeSliderItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeSliderLanguages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HomeSliderLanguages_HomeSliderItems_HomeSliderItemId",
                        column: x => x.HomeSliderItemId,
                        principalTable: "HomeSliderItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TopCategoryAreaLanguages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LangCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TopCategoryAreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopCategoryAreaLanguages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopCategoryAreaLanguages_TopCategoryAreas_TopCategoryAreaId",
                        column: x => x.TopCategoryAreaId,
                        principalTable: "TopCategoryAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DisCountAreaLanguages_DisCountAreaId",
                table: "DisCountAreaLanguages",
                column: "DisCountAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeSliderLanguages_HomeSliderItemId",
                table: "HomeSliderLanguages",
                column: "HomeSliderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TopCategoryAreaLanguages_TopCategoryAreaId",
                table: "TopCategoryAreaLanguages",
                column: "TopCategoryAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_TopCategoryAreas_CategoryId",
                table: "TopCategoryAreas",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DisCountAreaLanguages");

            migrationBuilder.DropTable(
                name: "HomeSliderLanguages");

            migrationBuilder.DropTable(
                name: "TopCategoryAreaLanguages");

            migrationBuilder.DropTable(
                name: "DisCountAreas");

            migrationBuilder.DropTable(
                name: "HomeSliderItems");

            migrationBuilder.DropTable(
                name: "TopCategoryAreas");
        }
    }
}
