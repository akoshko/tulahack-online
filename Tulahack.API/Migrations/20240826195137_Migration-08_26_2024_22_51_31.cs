using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tulahack.API.Migrations
{
    /// <inheritdoc />
    public partial class Migration08_26_2024_22_51_31 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContestApplications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Justification = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContestApplications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StorageFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Filename = table.Column<string>(type: "TEXT", nullable: true),
                    Filepath = table.Column<string>(type: "TEXT", nullable: true),
                    Owner = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Revision = table.Column<int>(type: "INTEGER", nullable: false),
                    Purpose = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AcceptanceCriteria",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Criteria = table.Column<string>(type: "TEXT", nullable: true),
                    ContestCaseId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcceptanceCriteria", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Firstname = table.Column<string>(type: "TEXT", nullable: false),
                    Middlename = table.Column<string>(type: "TEXT", nullable: true),
                    Lastname = table.Column<string>(type: "TEXT", nullable: false),
                    TelegramAccount = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    About = table.Column<string>(type: "TEXT", nullable: true),
                    PhotoUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Blocked = table.Column<bool>(type: "INTEGER", nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Age = table.Column<int>(type: "INTEGER", nullable: true),
                    CertificateNeeded = table.Column<bool>(type: "INTEGER", nullable: true),
                    ContestantNumber = table.Column<int>(type: "INTEGER", nullable: true),
                    CompanyName = table.Column<string>(type: "TEXT", nullable: true),
                    JobTitle = table.Column<string>(type: "TEXT", nullable: true),
                    AttendingFirstTime = table.Column<bool>(type: "INTEGER", nullable: true),
                    ApplicationConfirmationStatus = table.Column<bool>(type: "INTEGER", nullable: true),
                    ApplicationSubmissionDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Region = table.Column<string>(type: "TEXT", nullable: true),
                    City = table.Column<string>(type: "TEXT", nullable: true),
                    University = table.Column<string>(type: "TEXT", nullable: true),
                    UniversityDepartment = table.Column<string>(type: "TEXT", nullable: true),
                    UniversityAttendanceFormat = table.Column<string>(type: "TEXT", nullable: true),
                    Scholarship = table.Column<string>(type: "TEXT", nullable: true),
                    ApplicationNumber = table.Column<Guid>(type: "TEXT", nullable: true),
                    AttendanceType = table.Column<string>(type: "TEXT", nullable: true),
                    EducationType = table.Column<string>(type: "TEXT", nullable: true),
                    GithubUrl = table.Column<string>(type: "TEXT", nullable: true),
                    CompanyId = table.Column<Guid>(type: "TEXT", nullable: true),
                    WorkPosition = table.Column<string>(type: "TEXT", nullable: true),
                    Confirmed = table.Column<bool>(type: "INTEGER", nullable: true),
                    Badge = table.Column<bool>(type: "INTEGER", nullable: true),
                    AdditionalContactPerson = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SkillName = table.Column<string>(type: "TEXT", nullable: true),
                    ContestantId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ExpertId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skills_Accounts_ContestantId",
                        column: x => x.ContestantId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Skills_Accounts_ExpertId",
                        column: x => x.ExpertId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Section = table.Column<int>(type: "INTEGER", nullable: false),
                    FormOfParticipation = table.Column<int>(type: "INTEGER", nullable: false),
                    RepositoryLink = table.Column<string>(type: "TEXT", nullable: true),
                    AdditionalRepositoryLink = table.Column<string>(type: "TEXT", nullable: true),
                    PresentationLink = table.Column<string>(type: "TEXT", nullable: true),
                    VideoLink = table.Column<string>(type: "TEXT", nullable: true),
                    OpenUniversityProjectLink = table.Column<string>(type: "TEXT", nullable: true),
                    AdditionalInfo = table.Column<string>(type: "TEXT", nullable: true),
                    Teaser = table.Column<string>(type: "TEXT", nullable: true),
                    ExpertId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_Accounts_ExpertId",
                        column: x => x.ExpertId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ContestCases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    CompanyId = table.Column<Guid>(type: "TEXT", nullable: true),
                    TeamId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContestCases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContestCases_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ContestCases_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcceptanceCriteria_ContestCaseId",
                table: "AcceptanceCriteria",
                column: "ContestCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_CompanyId",
                table: "Accounts",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_TeamId",
                table: "Accounts",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_ContestCases_CompanyId",
                table: "ContestCases",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ContestCases_TeamId",
                table: "ContestCases",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_ContestantId",
                table: "Skills",
                column: "ContestantId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_ExpertId",
                table: "Skills",
                column: "ExpertId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_ExpertId",
                table: "Teams",
                column: "ExpertId");

            migrationBuilder.AddForeignKey(
                name: "FK_AcceptanceCriteria_ContestCases_ContestCaseId",
                table: "AcceptanceCriteria",
                column: "ContestCaseId",
                principalTable: "ContestCases",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Teams_TeamId",
                table: "Accounts",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Companies_CompanyId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Teams_TeamId",
                table: "Accounts");

            migrationBuilder.DropTable(
                name: "AcceptanceCriteria");

            migrationBuilder.DropTable(
                name: "ContestApplications");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "StorageFiles");

            migrationBuilder.DropTable(
                name: "ContestCases");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
