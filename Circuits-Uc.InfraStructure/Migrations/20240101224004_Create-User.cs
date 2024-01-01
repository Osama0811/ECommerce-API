using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CircuitsUc.InfraStructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SecurityUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlternativePhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreferedContact = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsOnline = table.Column<bool>(type: "bit", nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SecurityUsers_SecurityUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "SecurityUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SecurityUsers_SecurityUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "SecurityUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SecurityUsers_CreatedBy",
                table: "SecurityUsers",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityUsers_UpdatedBy",
                table: "SecurityUsers",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SecurityUsers");
        }
    }
}
