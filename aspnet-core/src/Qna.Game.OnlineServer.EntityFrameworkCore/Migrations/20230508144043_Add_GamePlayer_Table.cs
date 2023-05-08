using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qna.Game.OnlineServer.Migrations
{
    /// <inheritdoc />
    public partial class AddGamePlayerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "MaxPlayer",
                schema: "Game",
                table: "Game",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "MinPlayer",
                schema: "Game",
                table: "Game",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.CreateTable(
                name: "GamePlayer",
                schema: "Game",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    GameId = table.Column<long>(type: "bigint", nullable: false),
                    CurrentLevel = table.Column<int>(type: "integer", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePlayer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GamePlayer_Game_GameId",
                        column: x => x.GameId,
                        principalSchema: "Game",
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayer_GameId",
                schema: "Game",
                table: "GamePlayer",
                column: "GameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GamePlayer",
                schema: "Game");

            migrationBuilder.DropColumn(
                name: "MaxPlayer",
                schema: "Game",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "MinPlayer",
                schema: "Game",
                table: "Game");
        }
    }
}
