using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class IliskiZorunluluguGuncellendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StokHareketleri_Urunler_UrunId",
                table: "StokHareketleri");

            migrationBuilder.AddForeignKey(
                name: "FK_StokHareketleri_Urunler_UrunId",
                table: "StokHareketleri",
                column: "UrunId",
                principalTable: "Urunler",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StokHareketleri_Urunler_UrunId",
                table: "StokHareketleri");

            migrationBuilder.AddForeignKey(
                name: "FK_StokHareketleri_Urunler_UrunId",
                table: "StokHareketleri",
                column: "UrunId",
                principalTable: "Urunler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
