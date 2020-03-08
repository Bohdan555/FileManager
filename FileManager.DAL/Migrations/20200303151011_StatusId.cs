using Microsoft.EntityFrameworkCore.Migrations;

namespace TransactionManager.DAL.Migrations
{
    public partial class StatusId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Statuses_StatusId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_StatusId",
                table: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "Transactions",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_StatusId",
                table: "Transactions",
                column: "StatusId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Statuses_StatusId",
                table: "Transactions",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Statuses_StatusId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_StatusId",
                table: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "Transactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_StatusId",
                table: "Transactions",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Statuses_StatusId",
                table: "Transactions",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
