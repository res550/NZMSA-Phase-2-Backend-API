using Microsoft.EntityFrameworkCore.Migrations;

namespace MPapi.Migrations
{
    public partial class InitalCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ListingItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    userId = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Price = table.Column<string>(nullable: true),
                    Uploaded = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    Seller = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingItem", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListingItem");
        }
    }
}
