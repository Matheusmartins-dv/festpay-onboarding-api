using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Festpay.Onboarding.Infra.Migrations
{
    /// <inheritdoc />
    public partial class Update_struct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SourceAccountId = table.Column<string>(type: "TEXT", nullable: false),
                    DestinationAccountId = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsCanceled = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DeactivatedUtc = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
