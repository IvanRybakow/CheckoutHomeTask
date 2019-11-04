using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Checkout.HomeTask.Api.Data.Migrations
{
    public partial class AddedPaymentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BankPaymentId = table.Column<string>(nullable: true),
                    MerchantId = table.Column<string>(nullable: true),
                    MaskedCardNumber = table.Column<string>(nullable: true),
                    CardHolderName = table.Column<string>(nullable: true),
                    Amount = table.Column<int>(nullable: false),
                    Currency = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");
        }
    }
}
