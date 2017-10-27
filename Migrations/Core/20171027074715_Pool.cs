using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Core.Migrations.Core
{
    public partial class Pool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", maxLength: 8, nullable: true),
                    Group = table.Column<string>(type: "TEXT", maxLength: 1, nullable: false),
                    LastModified = table.Column<DateTime>(type: "DateTime", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Finals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LastModified = table.Column<DateTime>(type: "DateTime", nullable: false),
                    LevelName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LevelNumber = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Finals", x => x.Id);
                    table.UniqueConstraint("AK_Finals_LevelNumber", x => x.LevelNumber);
                });

            migrationBuilder.CreateTable(
                name: "FinalsPlacing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CountryId = table.Column<int>(type: "INTEGER", nullable: false),
                    FinalsId = table.Column<int>(type: "INTEGER", nullable: true),
                    LastModified = table.Column<DateTime>(type: "DateTime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinalsPlacing", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Match",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Country1Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Country2Id = table.Column<int>(type: "INTEGER", nullable: false),
                    GoalsCountry1 = table.Column<int>(type: "INTEGER", nullable: false),
                    GoalsCountry2 = table.Column<int>(type: "INTEGER", nullable: false),
                    LastModified = table.Column<DateTime>(type: "DateTime", nullable: false),
                    Location = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    StartDate = table.Column<DateTime>(type: "DateTime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Match", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PoolPlayer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LastModified = table.Column<DateTime>(type: "DateTime", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    OpenQuestions = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    SubScore = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoolPlayer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PoolPlayer_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MatchFinals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Country1Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Country1Text = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Country2Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Country2Text = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    GoalsCountry1 = table.Column<int>(type: "INTEGER", nullable: false),
                    GoalsCountry2 = table.Column<int>(type: "INTEGER", nullable: false),
                    LastModified = table.Column<DateTime>(type: "DateTime", nullable: false),
                    LevelNumber = table.Column<int>(type: "INTEGER", nullable: true),
                    Location = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    StartDate = table.Column<DateTime>(type: "DateTime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchFinals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchFinals_Finals_LevelNumber",
                        column: x => x.LevelNumber,
                        principalTable: "Finals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FinalsPrediction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CountryId = table.Column<int>(type: "INTEGER", nullable: false),
                    FinalsId = table.Column<int>(type: "INTEGER", nullable: false),
                    LastModified = table.Column<DateTime>(type: "DateTime", nullable: false),
                    PoolPlayerId = table.Column<int>(type: "INTEGER", nullable: false),
                    SubScore = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinalsPrediction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinalsPrediction_PoolPlayer_PoolPlayerId",
                        column: x => x.PoolPlayerId,
                        principalTable: "PoolPlayer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchPrediction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GoalsCountry1 = table.Column<int>(type: "INTEGER", nullable: false),
                    GoalsCountry2 = table.Column<int>(type: "INTEGER", nullable: false),
                    LastModified = table.Column<DateTime>(type: "DateTime", nullable: false),
                    MatchId = table.Column<int>(type: "INTEGER", nullable: false),
                    PoolPlayerId = table.Column<int>(type: "INTEGER", nullable: false),
                    SubScore = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchPrediction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchPrediction_PoolPlayer_PoolPlayerId",
                        column: x => x.PoolPlayerId,
                        principalTable: "PoolPlayer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PoolMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LastModified = table.Column<DateTime>(type: "DateTime", nullable: false),
                    Message = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    PlacedDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    PoolPlayerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoolMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PoolMessage_PoolPlayer_PoolPlayerId",
                        column: x => x.PoolPlayerId,
                        principalTable: "PoolPlayer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinalsPrediction_PoolPlayerId",
                table: "FinalsPrediction",
                column: "PoolPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchFinals_LevelNumber",
                table: "MatchFinals",
                column: "LevelNumber");

            migrationBuilder.CreateIndex(
                name: "IX_MatchPrediction_PoolPlayerId",
                table: "MatchPrediction",
                column: "PoolPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PoolMessage_PoolPlayerId",
                table: "PoolMessage",
                column: "PoolPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PoolPlayer_UserId",
                table: "PoolPlayer",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "FinalsPlacing");

            migrationBuilder.DropTable(
                name: "FinalsPrediction");

            migrationBuilder.DropTable(
                name: "Match");

            migrationBuilder.DropTable(
                name: "MatchFinals");

            migrationBuilder.DropTable(
                name: "MatchPrediction");

            migrationBuilder.DropTable(
                name: "PoolMessage");

            migrationBuilder.DropTable(
                name: "Finals");

            migrationBuilder.DropTable(
                name: "PoolPlayer");
        }
    }
}
