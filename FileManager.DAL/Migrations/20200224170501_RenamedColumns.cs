using Microsoft.EntityFrameworkCore.Migrations;

namespace TransactionManager.DAL.Migrations
{
    public partial class RenamedColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Transaction Id",
                table: "Transactions",
                newName: "TransactionId");

            migrationBuilder.RenameColumn(
                name: "Transaction Date",
                table: "Transactions",
                newName: "TransactionDate");

            migrationBuilder.RenameColumn(
                name: "Currency Code",
                table: "Transactions",
                newName: "CurrencyCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "Transactions",
                newName: "Transaction Id");

            migrationBuilder.RenameColumn(
                name: "TransactionDate",
                table: "Transactions",
                newName: "Transaction Date");

            migrationBuilder.RenameColumn(
                name: "CurrencyCode",
                table: "Transactions",
                newName: "Currency Code");
        }
    }
}
