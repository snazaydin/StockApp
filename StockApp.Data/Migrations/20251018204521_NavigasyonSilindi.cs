using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class NavigasyonSilindi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StokHareketleri_Urunler_UrunId",
                table: "StokHareketleri");

            migrationBuilder.DropIndex(
                name: "IX_StokHareketleri_UrunId",
                table: "StokHareketleri");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_StokHareketleri_UrunId",
                table: "StokHareketleri",
                column: "UrunId");

            migrationBuilder.AddForeignKey(
                name: "FK_StokHareketleri_Urunler_UrunId",
                table: "StokHareketleri",
                column: "UrunId",
                principalTable: "Urunler",
                principalColumn: "Id");
        }
    }
}
