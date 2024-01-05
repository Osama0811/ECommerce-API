using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CircuitsUc.InfraStructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateTablePageContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PageContents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ShortDescriptionAr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ShortDescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeID = table.Column<int>(type: "int", nullable: false),
                    PostedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Rank = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PageContents_SecurityUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "SecurityUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PageContents_SecurityUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "SecurityUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PageContents_CreatedBy",
                table: "PageContents",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PageContents_UpdatedBy",
                table: "PageContents",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PageContents");
        }
    }
}
