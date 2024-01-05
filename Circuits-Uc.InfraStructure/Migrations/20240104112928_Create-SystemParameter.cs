using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CircuitsUc.InfraStructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateSystemParameter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SystemParameters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SettingKey = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SettingValueEN = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SettingValueAR = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsSystemKey = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemParameters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemParameters_SecurityUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "SecurityUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SystemParameters_SecurityUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "SecurityUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SystemParameters_CreatedBy",
                table: "SystemParameters",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SystemParameters_UpdatedBy",
                table: "SystemParameters",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemParameters");
        }
    }
}
