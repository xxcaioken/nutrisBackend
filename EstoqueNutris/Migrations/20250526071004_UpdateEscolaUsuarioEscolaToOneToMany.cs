using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstoqueNutris.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEscolaUsuarioEscolaToOneToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Escolas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Endereco = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Telefone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Escolas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioEscolas_EscolaId",
                table: "UsuarioEscolas",
                column: "EscolaId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioEscolas_Escolas_EscolaId",
                table: "UsuarioEscolas",
                column: "EscolaId",
                principalTable: "Escolas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioEscolas_Escolas_EscolaId",
                table: "UsuarioEscolas");

            migrationBuilder.DropTable(
                name: "Escolas");

            migrationBuilder.DropIndex(
                name: "IX_UsuarioEscolas_EscolaId",
                table: "UsuarioEscolas");
        }
    }
}
