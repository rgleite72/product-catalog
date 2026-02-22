using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial_ProductCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    sku = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "product_prices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    valid_from = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    valid_to = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_prices", x => x.Id);
                    table.CheckConstraint("ck_product_prices_amount_nonnegative", "amount >=0");
                    table.ForeignKey(
                        name: "FK_product_prices_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_stocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity_on_hand = table.Column<int>(type: "integer", nullable: false),
                    reserved_quantity = table.Column<int>(type: "integer", nullable: false),
                    min_quantity = table.Column<int>(type: "integer", nullable: true),
                    UpdateAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_stocks", x => x.Id);
                    table.CheckConstraint("ck_product_stocks_qty_nonnegative", "quantity_on_hand >= 0");
                    table.CheckConstraint("ck_product_stocks_reserved_nonnegative", "reserved_quantity >= 0");
                    table.ForeignKey(
                        name: "FK_product_stocks_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_product_prices_product_id_valid_from",
                table: "product_prices",
                columns: new[] { "product_id", "valid_from" });

            migrationBuilder.CreateIndex(
                name: "ux_product_stocks_product_id",
                table: "product_stocks",
                column: "product_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_products_sku",
                table: "products",
                column: "sku",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_prices");

            migrationBuilder.DropTable(
                name: "product_stocks");

            migrationBuilder.DropTable(
                name: "products");
        }
    }
}
