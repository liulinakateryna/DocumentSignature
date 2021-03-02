using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.KeyCenter.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Keys",
                columns: table => new
                {
                    KeysId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    D = table.Column<byte[]>(nullable: true),
                    DP = table.Column<byte[]>(nullable: true),
                    DQ = table.Column<byte[]>(nullable: true),
                    Exponent = table.Column<byte[]>(nullable: true),
                    InverseQ = table.Column<byte[]>(nullable: true),
                    Modulus = table.Column<byte[]>(nullable: true),
                    P = table.Column<byte[]>(nullable: true),
                    Q = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keys", x => x.KeysId);
                });

            migrationBuilder.CreateTable(
                name: "PrivateData",
                columns: table => new
                {
                    PrivateDataId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true),
                    Fingerprint = table.Column<byte[]>(nullable: true),
                    KeysId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateData", x => x.PrivateDataId);
                    table.ForeignKey(
                        name: "FK_PrivateData_Keys_KeysId",
                        column: x => x.KeysId,
                        principalTable: "Keys",
                        principalColumn: "KeysId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrivateData_KeysId",
                table: "PrivateData",
                column: "KeysId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrivateData");

            migrationBuilder.DropTable(
                name: "Keys");
        }
    }
}
