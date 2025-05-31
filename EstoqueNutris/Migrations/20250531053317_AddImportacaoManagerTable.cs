using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EstoqueNutris.Migrations
{
    /// <inheritdoc />
    public partial class AddImportacaoManagerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImportacaoManagers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsuarioEscolaId = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PlanilhaOrigemUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    PlanilhaDestinoUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CelulasMapping = table.Column<string>(type: "text", nullable: false),
                    Descricao = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsAtivo = table.Column<bool>(type: "boolean", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    DataAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UltimaExecucao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportacaoManagers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportacaoManagers_UsuarioEscolas_UsuarioEscolaId",
                        column: x => x.UsuarioEscolaId,
                        principalTable: "UsuarioEscolas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImportacaoManagers_UsuarioEscolaId",
                table: "ImportacaoManagers",
                column: "UsuarioEscolaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImportacaoManagers");
        }
    }
}
