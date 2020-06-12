using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal.Mapping;
using NpgsqlTypes;

namespace Gear.Persistence.Migrations
{
    public partial class Postgress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "ReferenceSequenceActivities");

            migrationBuilder.CreateSequence<int>(
                name: "ReferenceSequenceActivityLists");

            migrationBuilder.CreateSequence<int>(
                name: "ReferenceSequenceChangeRequests");

            migrationBuilder.CreateSequence<int>(
                name: "ReferenceSequenceProjects");

            migrationBuilder.CreateTable(
                name: "ActivityTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    IsDeletable = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Abbreviation = table.Column<string>(nullable: true),
                    ColorBadge = table.Column<int>(nullable: false),
                    RowOrder = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AuditUserId = table.Column<Guid>(nullable: false),
                    AuditUserName = table.Column<string>(nullable: false),
                    EntityPk = table.Column<string>(nullable: false),
                    TableName = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    AuditDate = table.Column<DateTime>(nullable: false),
                    EntityTypeName = table.Column<string>(nullable: true),
                    AuditData = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobPositions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    IsDeletable = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    HourlySalary = table.Column<int>(nullable: true),
                    Abbreviation = table.Column<string>(nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    RowOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobPositions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MainComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AuthorId = table.Column<Guid>(nullable: false),
                    AuthorName = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    RecordId = table.Column<Guid>(nullable: false),
                    AuthorJobPosition = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainComments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModulesForUsers",
                columns: table => new
                {
                    UserId = table.Column<Guid>(maxLength: 450, nullable: false),
                    AllowedPaidForModules = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModulesForUsers", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Platforms",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platforms", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "ProductTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    IsDeletable = table.Column<bool>(nullable: false, defaultValue: true),
                    Active = table.Column<bool>(nullable: false, defaultValue: true),
                    RowOrder = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    IsDeletable = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecruitingPipelines",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    IsDeletable = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecruitingPipelines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolesToPermissions",
                columns: table => new
                {
                    RoleName = table.Column<string>(maxLength: 450, nullable: false),
                    PermissionsInRole = table.Column<string>(nullable: true),
                    PlatformName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesToPermissions", x => x.RoleName);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTimeCheckers",
                columns: table => new
                {
                    ServiceId = table.Column<Guid>(nullable: false),
                    ServiceName = table.Column<string>(nullable: false),
                    ExecutedLastTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTimeCheckers", x => x.ServiceId);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    IsDeletable = table.Column<bool>(nullable: false, defaultValue: true),
                    Active = table.Column<bool>(nullable: false, defaultValue: true),
                    RowOrder = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SolutionTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    IsDeletable = table.Column<bool>(nullable: false, defaultValue: true),
                    Active = table.Column<bool>(nullable: false, defaultValue: true),
                    RowOrder = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolutionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TableHeaders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Width = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false, defaultValue: true),
                    Deletable = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableHeaders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TechnologyTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    IsDeletable = table.Column<bool>(nullable: false, defaultValue: true),
                    Active = table.Column<bool>(nullable: false, defaultValue: true),
                    RowOrder = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnologyTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrackerTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    IsDeletable = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    ActivityTypeId = table.Column<Guid>(nullable: true),
                    RowOrder = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackerTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackerTypes_ActivityTypes_ActivityTypeId",
                        column: x => x.ActivityTypeId,
                        principalTable: "ActivityTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntryChanges",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AuditLogId = table.Column<Guid>(nullable: false),
                    ColumnName = table.Column<string>(nullable: true),
                    OriginalValue = table.Column<string>(nullable: true),
                    NewValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntryChanges_AuditLogs_AuditLogId",
                        column: x => x.AuditLogId,
                        principalTable: "AuditLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<NpgsqlDbType>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    EmploymentType = table.Column<int>(nullable: false),
                    JobPositionId = table.Column<Guid>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_JobPositions_JobPositionId",
                        column: x => x.JobPositionId,
                        principalTable: "JobPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AuthorId = table.Column<Guid>(nullable: false),
                    AuthorName = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    AuthorJobPosition = table.Column<string>(nullable: true),
                    MainCommentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubComments_MainComments_MainCommentId",
                        column: x => x.MainCommentId,
                        principalTable: "MainComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PlatformName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoles_Platforms_PlatformName",
                        column: x => x.PlatformName,
                        principalTable: "Platforms",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecruitmentStages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    IsDeletable = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    PipelineId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecruitmentStages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecruitmentStages_RecruitingPipelines_PipelineId",
                        column: x => x.PipelineId,
                        principalTable: "RecruitingPipelines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusinessUnits",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    IsDeletable = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(maxLength: 900, nullable: true),
                    BusinessUnitLeadId = table.Column<Guid>(nullable: true),
                    Address = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessUnits_AspNetUsers_BusinessUnitLeadId",
                        column: x => x.BusinessUnitLeadId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    IsDeletable = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    ProjectSettingsId = table.Column<Guid>(nullable: false),
                    ProjectInvoiceId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    ProjectUrl = table.Column<string>(maxLength: 100, nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    Budget = table.Column<decimal>(nullable: true),
                    ProjectGroupId = table.Column<Guid>(nullable: false),
                    ProjectManagerId = table.Column<Guid>(nullable: true),
                    Status = table.Column<int>(nullable: false, defaultValue: 1),
                    Priority = table.Column<int>(nullable: false, defaultValue: 1),
                    ServiceTypeId = table.Column<Guid>(nullable: true),
                    SolutionTypeId = table.Column<Guid>(nullable: true),
                    ProductTypeId = table.Column<Guid>(nullable: true),
                    TechnologyTypeId = table.Column<Guid>(nullable: true),
                    Number = table.Column<int>(nullable: true, defaultValueSql: "nextval('\"ReferenceSequenceProjects\"')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_ProductTypes_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalTable: "ProductTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_ProjectGroups_ProjectGroupId",
                        column: x => x.ProjectGroupId,
                        principalTable: "ProjectGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_AspNetUsers_ProjectManagerId",
                        column: x => x.ProjectManagerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_ServiceTypes_ServiceTypeId",
                        column: x => x.ServiceTypeId,
                        principalTable: "ServiceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_SolutionTypes_SolutionTypeId",
                        column: x => x.SolutionTypeId,
                        principalTable: "SolutionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_TechnologyTypes_TechnologyTypeId",
                        column: x => x.TechnologyTypeId,
                        principalTable: "TechnologyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReportFilters",
                columns: table => new
                {
                    ReportId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    FilterType = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportFilters", x => new { x.UserId, x.ReportId, x.FilterType });
                    table.ForeignKey(
                        name: "FK_ReportFilters_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportFilters_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportTableHeaders",
                columns: table => new
                {
                    ReportId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    TableHeaderId = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportTableHeaders", x => new { x.UserId, x.ReportId, x.TableHeaderId });
                    table.ForeignKey(
                        name: "FK_ReportTableHeaders_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportTableHeaders_TableHeaders_TableHeaderId",
                        column: x => x.TableHeaderId,
                        principalTable: "TableHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportTableHeaders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserActivityStatusFilters",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ReportId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    ActivityStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActivityStatusFilters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserActivityStatusFilters_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserActivityStatusFilters_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDateFilters",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ReportId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    FilterType = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDateFilters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDateFilters_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDateFilters_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserGuidFilters",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ReportId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    FilterType = table.Column<int>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGuidFilters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGuidFilters_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGuidFilters_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProjectStatusFilters",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ReportId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    ProjectStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProjectStatusFilters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProjectStatusFilters_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProjectStatusFilters_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserReports",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    ReportId = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReports", x => new { x.UserId, x.ReportId });
                    table.ForeignKey(
                        name: "FK_UserReports_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserReports_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    RoleId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Candidates",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    IsDeletable = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    ContactInfo_PhoneNumber = table.Column<string>(nullable: true),
                    ContactInfo_Email = table.Column<string>(nullable: true),
                    JobPositionId = table.Column<Guid>(nullable: true),
                    CandidateStatus = table.Column<int>(nullable: false),
                    DesiredSalary = table.Column<decimal>(type: "MONEY", nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    RecruitmentStageId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Candidates_JobPositions_JobPositionId",
                        column: x => x.JobPositionId,
                        principalTable: "JobPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Candidates_RecruitmentStages_RecruitmentStageId",
                        column: x => x.RecruitmentStageId,
                        principalTable: "RecruitmentStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    IsDeletable = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(maxLength: 900, nullable: true),
                    BusinessUnitId = table.Column<Guid>(nullable: true),
                    DepartmentLeadId = table.Column<Guid>(nullable: true),
                    Abbreviation = table.Column<string>(nullable: true),
                    RowOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_BusinessUnits_BusinessUnitId",
                        column: x => x.BusinessUnitId,
                        principalTable: "BusinessUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Departments_AspNetUsers_DepartmentLeadId",
                        column: x => x.DepartmentLeadId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ChangeRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    IsDeletable = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ProjectId = table.Column<Guid>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValue: 1),
                    Number = table.Column<int>(nullable: false, defaultValueSql: "nextval('\"ReferenceSequenceChangeRequests\"')"),
                    Comment = table.Column<string>(nullable: true),
                    ReviewBy = table.Column<Guid>(nullable: false),
                    ReviewDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChangeRequests_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectActivityTypes",
                columns: table => new
                {
                    ProjectId = table.Column<Guid>(nullable: false),
                    ActivityTypeId = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false, defaultValue: true),
                    ModifiedBy = table.Column<Guid>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectActivityTypes", x => new { x.ProjectId, x.ActivityTypeId });
                    table.ForeignKey(
                        name: "FK_ProjectActivityTypes_ActivityTypes_ActivityTypeId",
                        column: x => x.ActivityTypeId,
                        principalTable: "ActivityTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectActivityTypes_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectInvoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    Currency = table.Column<int>(nullable: false),
                    BudgetMoney = table.Column<decimal>(nullable: false),
                    HourRateMoney = table.Column<decimal>(nullable: false),
                    BudgetMoneyHours = table.Column<decimal>(nullable: false),
                    MaxLogHoursDay = table.Column<int>(nullable: false),
                    MaxLogHoursActivity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectInvoices_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectMembers",
                columns: table => new
                {
                    ProjectId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMembers", x => new { x.ProjectId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ProjectMembers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectMembers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    IsDeletable = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    DailyMembersLoggedTime = table.Column<bool>(nullable: false),
                    WeeklyMembersLoggedTime = table.Column<bool>(nullable: false),
                    MonthlyMembersLoggedTime = table.Column<bool>(nullable: false),
                    DailyEmailsLoggedTimeActivityType = table.Column<bool>(nullable: false),
                    WeeklyEmailsLoggedTimeActivityType = table.Column<bool>(nullable: false),
                    MonthlyEmailsLoggedTimeActivityType = table.Column<bool>(nullable: false),
                    DailyEmailsLoggedTimeSprint = table.Column<bool>(nullable: false),
                    WeeklyEmailsLoggedTimeSprint = table.Column<bool>(nullable: false),
                    MonthlyEmailsLoggedTimeSprint = table.Column<bool>(nullable: false),
                    ProjectProjectTab = table.Column<bool>(nullable: false),
                    ProjectDashboardTab = table.Column<bool>(nullable: false, defaultValue: false),
                    ActivityTypesTab = table.Column<bool>(nullable: false, defaultValue: true),
                    ProjectActivitiesTab = table.Column<bool>(nullable: false),
                    ProjectLoggedTimeTab = table.Column<bool>(nullable: false),
                    ProjectChangeRequestsTab = table.Column<bool>(nullable: false),
                    ProjectReportsTab = table.Column<bool>(nullable: false),
                    ProjectInvoiceTab = table.Column<bool>(nullable: false),
                    ProjectWikiAndFilesTab = table.Column<bool>(nullable: false),
                    ActivityActivityTab = table.Column<bool>(nullable: false),
                    ActivityLinkedActivitiesTab = table.Column<bool>(nullable: false),
                    ActivityCommentsTab = table.Column<bool>(nullable: false),
                    ActivityHistoryTab = table.Column<bool>(nullable: false),
                    ActivityLoggedTimeTab = table.Column<bool>(nullable: false),
                    ProjectNotificationOnUpdateCompleteArchive = table.Column<bool>(nullable: false),
                    ProjectNotificationWeeklyDeadline = table.Column<bool>(nullable: false),
                    ProjectNotificationDailyDeadline = table.Column<bool>(nullable: false),
                    ProjectNotificationDailyOverdue = table.Column<bool>(nullable: false),
                    ActivityNotificationOnCreateUpdateCompleteDelete = table.Column<bool>(nullable: false),
                    ActivityNotificationWeeklyDeadline = table.Column<bool>(nullable: false),
                    ActivityNotificationDailyDeadline = table.Column<bool>(nullable: false),
                    ActivityNotificationDailyOverdue = table.Column<bool>(nullable: false),
                    SprintNotificationOnCreateUpdateCompleteDelete = table.Column<bool>(nullable: false),
                    SprintNotificationWeeklyDeadline = table.Column<bool>(nullable: false),
                    SprintNotificationDailyDeadline = table.Column<bool>(nullable: false),
                    SprintNotificationDailyOverdue = table.Column<bool>(nullable: false),
                    ActivityChangeName = table.Column<bool>(nullable: false),
                    ActivityChangeProject = table.Column<bool>(nullable: false),
                    ActivityChangeStatus = table.Column<bool>(nullable: false),
                    ActivityChangeProirity = table.Column<bool>(nullable: false),
                    ActivityChangeTeam = table.Column<bool>(nullable: false),
                    ActivityChangeEstimatedTime = table.Column<bool>(nullable: false),
                    ActivityChangeActivityType = table.Column<bool>(nullable: false),
                    ActivityChangeActivityList = table.Column<bool>(nullable: false),
                    ActivityChangeSprint = table.Column<bool>(nullable: false),
                    ActivityChangeStartDueDate = table.Column<bool>(nullable: false),
                    ActivityDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectSettings_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sprints",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    IsDeletable = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: true),
                    SprintStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sprints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sprints_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "WikiHeadlines",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    IsDeletable = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WikiHeadlines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WikiHeadlines_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentTeams",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    IsDeletable = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: false, defaultValue: true),
                    Description = table.Column<string>(maxLength: 900, nullable: true),
                    DepartmentId = table.Column<Guid>(nullable: true),
                    DepartmentTeamLeadId = table.Column<Guid>(nullable: true),
                    Abbreviation = table.Column<string>(nullable: true),
                    RowOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepartmentTeams_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DepartmentTeams_AspNetUsers_DepartmentTeamLeadId",
                        column: x => x.DepartmentTeamLeadId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ActivityLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    IsDeletable = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    ActivityListStatus = table.Column<int>(nullable: false),
                    SprintId = table.Column<Guid>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    DueDate = table.Column<DateTime>(nullable: true),
                    Number = table.Column<int>(nullable: false, defaultValueSql: "nextval('\"ReferenceSequenceActivityLists\"')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityLists_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityLists_Sprints_SprintId",
                        column: x => x.SprintId,
                        principalTable: "Sprints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    IsDeletable = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    HeadlineId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sections_WikiHeadlines_HeadlineId",
                        column: x => x.HeadlineId,
                        principalTable: "WikiHeadlines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobDepartmentTeams",
                columns: table => new
                {
                    DepartmentTeamId = table.Column<Guid>(nullable: false),
                    JobPositionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobDepartmentTeams", x => new { x.JobPositionId, x.DepartmentTeamId });
                    table.ForeignKey(
                        name: "FK_JobDepartmentTeams_DepartmentTeams_DepartmentTeamId",
                        column: x => x.DepartmentTeamId,
                        principalTable: "DepartmentTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobDepartmentTeams_JobPositions_JobPositionId",
                        column: x => x.JobPositionId,
                        principalTable: "JobPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDepartmentTeams",
                columns: table => new
                {
                    DeparmentTeamId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDepartmentTeams", x => new { x.DeparmentTeamId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserDepartmentTeams_DepartmentTeams_DeparmentTeamId",
                        column: x => x.DeparmentTeamId,
                        principalTable: "DepartmentTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDepartmentTeams_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    IsDeletable = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ActivityPriority = table.Column<int>(nullable: false),
                    ActivityStatus = table.Column<int>(nullable: false),
                    Progress = table.Column<int>(nullable: false),
                    EstimatedHours = table.Column<float>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    DueDate = table.Column<DateTime>(nullable: true),
                    ActivityListId = table.Column<Guid>(nullable: true),
                    SprintId = table.Column<Guid>(nullable: true),
                    ActivityTypeId = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    RowOrder = table.Column<int>(nullable: true),
                    Number = table.Column<int>(nullable: false, defaultValueSql: "nextval('\"ReferenceSequenceActivities\"')"),
                    ChnageRequestId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activities_ActivityLists_ActivityListId",
                        column: x => x.ActivityListId,
                        principalTable: "ActivityLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Activities_ActivityTypes_ActivityTypeId",
                        column: x => x.ActivityTypeId,
                        principalTable: "ActivityTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Activities_ChangeRequests_ChnageRequestId",
                        column: x => x.ChnageRequestId,
                        principalTable: "ChangeRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Activities_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Activities_Sprints_SprintId",
                        column: x => x.SprintId,
                        principalTable: "Sprints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ActivityAssignees",
                columns: table => new
                {
                    ActivityId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityAssignees", x => new { x.ActivityId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ActivityAssignees_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityAssignees_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoggedTimes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    IsDeletable = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Time = table.Column<float>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    ActivityId = table.Column<Guid>(nullable: false),
                    TrackerId = table.Column<Guid>(nullable: false),
                    DateOfWork = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoggedTimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoggedTimes_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LoggedTimes_TrackerTypes_TrackerId",
                        column: x => x.TrackerId,
                        principalTable: "TrackerTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LoggedTimes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CheckItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<Guid>(nullable: false),
                    ModifyTime = table.Column<DateTime>(nullable: false),
                    IsDeletable = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    IsCompleted = table.Column<bool>(nullable: false),
                    ActivityId = table.Column<Guid>(nullable: false),
                    LoggedTimeId = table.Column<Guid>(nullable: true),
                    ApplicationUserId = table.Column<Guid>(nullable: false),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckItems_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckItems_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckItems_LoggedTimes_LoggedTimeId",
                        column: x => x.LoggedTimeId,
                        principalTable: "LoggedTimes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ActivityListId",
                table: "Activities",
                column: "ActivityListId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ActivityTypeId",
                table: "Activities",
                column: "ActivityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ChnageRequestId",
                table: "Activities",
                column: "ChnageRequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ProjectId",
                table: "Activities",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_SprintId",
                table: "Activities",
                column: "SprintId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityAssignees_UserId",
                table: "ActivityAssignees",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLists_ProjectId",
                table: "ActivityLists",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLists_SprintId",
                table: "ActivityLists",
                column: "SprintId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_PlatformName",
                table: "AspNetRoles",
                column: "PlatformName");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_JobPositionId",
                table: "AspNetUsers",
                column: "JobPositionId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessUnits_BusinessUnitLeadId",
                table: "BusinessUnits",
                column: "BusinessUnitLeadId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_JobPositionId",
                table: "Candidates",
                column: "JobPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_RecruitmentStageId",
                table: "Candidates",
                column: "RecruitmentStageId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequests_ProjectId",
                table: "ChangeRequests",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckItems_ActivityId",
                table: "CheckItems",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckItems_ApplicationUserId",
                table: "CheckItems",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckItems_LoggedTimeId",
                table: "CheckItems",
                column: "LoggedTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_BusinessUnitId",
                table: "Departments",
                column: "BusinessUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_DepartmentLeadId",
                table: "Departments",
                column: "DepartmentLeadId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentTeams_DepartmentId",
                table: "DepartmentTeams",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentTeams_DepartmentTeamLeadId",
                table: "DepartmentTeams",
                column: "DepartmentTeamLeadId");

            migrationBuilder.CreateIndex(
                name: "IX_EntryChanges_AuditLogId",
                table: "EntryChanges",
                column: "AuditLogId");

            migrationBuilder.CreateIndex(
                name: "IX_JobDepartmentTeams_DepartmentTeamId",
                table: "JobDepartmentTeams",
                column: "DepartmentTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_LoggedTimes_ActivityId",
                table: "LoggedTimes",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_LoggedTimes_TrackerId",
                table: "LoggedTimes",
                column: "TrackerId");

            migrationBuilder.CreateIndex(
                name: "IX_LoggedTimes_UserId",
                table: "LoggedTimes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MainComments_RecordId",
                table: "MainComments",
                column: "RecordId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectActivityTypes_ActivityTypeId",
                table: "ProjectActivityTypes",
                column: "ActivityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectInvoices_ProjectId",
                table: "ProjectInvoices",
                column: "ProjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_UserId",
                table: "ProjectMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProductTypeId",
                table: "Projects",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectGroupId",
                table: "Projects",
                column: "ProjectGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectManagerId",
                table: "Projects",
                column: "ProjectManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ServiceTypeId",
                table: "Projects",
                column: "ServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_SolutionTypeId",
                table: "Projects",
                column: "SolutionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_TechnologyTypeId",
                table: "Projects",
                column: "TechnologyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSettings_ProjectId",
                table: "ProjectSettings",
                column: "ProjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecruitmentStages_PipelineId",
                table: "RecruitmentStages",
                column: "PipelineId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportFilters_ReportId",
                table: "ReportFilters",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportTableHeaders_ReportId",
                table: "ReportTableHeaders",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportTableHeaders_TableHeaderId",
                table: "ReportTableHeaders",
                column: "TableHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_HeadlineId",
                table: "Sections",
                column: "HeadlineId");

            migrationBuilder.CreateIndex(
                name: "IX_Sprints_ProjectId",
                table: "Sprints",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SubComments_MainCommentId",
                table: "SubComments",
                column: "MainCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackerTypes_ActivityTypeId",
                table: "TrackerTypes",
                column: "ActivityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserActivityStatusFilters_ReportId",
                table: "UserActivityStatusFilters",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_UserActivityStatusFilters_UserId",
                table: "UserActivityStatusFilters",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDateFilters_ReportId",
                table: "UserDateFilters",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDateFilters_UserId",
                table: "UserDateFilters",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDepartmentTeams_UserId",
                table: "UserDepartmentTeams",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGuidFilters_ReportId",
                table: "UserGuidFilters",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGuidFilters_UserId",
                table: "UserGuidFilters",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProjectStatusFilters_ReportId",
                table: "UserProjectStatusFilters",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProjectStatusFilters_UserId",
                table: "UserProjectStatusFilters",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserReports_ReportId",
                table: "UserReports",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_WikiHeadlines_ProjectId",
                table: "WikiHeadlines",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityAssignees");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Candidates");

            migrationBuilder.DropTable(
                name: "CheckItems");

            migrationBuilder.DropTable(
                name: "EntryChanges");

            migrationBuilder.DropTable(
                name: "JobDepartmentTeams");

            migrationBuilder.DropTable(
                name: "ModulesForUsers");

            migrationBuilder.DropTable(
                name: "ProjectActivityTypes");

            migrationBuilder.DropTable(
                name: "ProjectInvoices");

            migrationBuilder.DropTable(
                name: "ProjectMembers");

            migrationBuilder.DropTable(
                name: "ProjectSettings");

            migrationBuilder.DropTable(
                name: "ReportFilters");

            migrationBuilder.DropTable(
                name: "ReportTableHeaders");

            migrationBuilder.DropTable(
                name: "RolesToPermissions");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "ServiceTimeCheckers");

            migrationBuilder.DropTable(
                name: "SubComments");

            migrationBuilder.DropTable(
                name: "UserActivityStatusFilters");

            migrationBuilder.DropTable(
                name: "UserDateFilters");

            migrationBuilder.DropTable(
                name: "UserDepartmentTeams");

            migrationBuilder.DropTable(
                name: "UserGuidFilters");

            migrationBuilder.DropTable(
                name: "UserProjectStatusFilters");

            migrationBuilder.DropTable(
                name: "UserReports");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "RecruitmentStages");

            migrationBuilder.DropTable(
                name: "LoggedTimes");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "TableHeaders");

            migrationBuilder.DropTable(
                name: "WikiHeadlines");

            migrationBuilder.DropTable(
                name: "MainComments");

            migrationBuilder.DropTable(
                name: "DepartmentTeams");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "Platforms");

            migrationBuilder.DropTable(
                name: "RecruitingPipelines");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "TrackerTypes");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "ActivityLists");

            migrationBuilder.DropTable(
                name: "ChangeRequests");

            migrationBuilder.DropTable(
                name: "ActivityTypes");

            migrationBuilder.DropTable(
                name: "BusinessUnits");

            migrationBuilder.DropTable(
                name: "Sprints");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "ProductTypes");

            migrationBuilder.DropTable(
                name: "ProjectGroups");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ServiceTypes");

            migrationBuilder.DropTable(
                name: "SolutionTypes");

            migrationBuilder.DropTable(
                name: "TechnologyTypes");

            migrationBuilder.DropTable(
                name: "JobPositions");

            migrationBuilder.DropSequence(
                name: "ReferenceSequenceActivities");

            migrationBuilder.DropSequence(
                name: "ReferenceSequenceActivityLists");

            migrationBuilder.DropSequence(
                name: "ReferenceSequenceChangeRequests");

            migrationBuilder.DropSequence(
                name: "ReferenceSequenceProjects");
        }
    }
}
