using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LunarApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "Notebooks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Notebook Identifier"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Notebook title"),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 20000, nullable: true, comment: "Notebook description"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "User Identifier")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notebooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notebooks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Tag Identifier"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Tag name"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "User Identifier")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Folders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Folder Identifier"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Folder title"),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 20000, nullable: true, comment: "Folder description"),
                    NotebookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identifier of a notebook"),
                    ParentFolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Identifier of a parent folder"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "User Identifier")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Folders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Folders_Folders_ParentFolderId",
                        column: x => x.ParentFolderId,
                        principalTable: "Folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Folders_Notebooks_NotebookId",
                        column: x => x.NotebookId,
                        principalTable: "Notebooks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Note Identifier"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Note title"),
                    Body = table.Column<string>(type: "nvarchar(max)", maxLength: 20000, nullable: true, comment: "Note body"),
                    NotebookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identifier of a notebook"),
                    ParentFolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Identifier of a parent folder"),
                    FolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Identifier of a folder"),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Identifier of a tag"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "User Identifier"),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "The date the note was created on"),
                    LastSaved = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "The date the note was last saved on")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notes_Folders_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notes_Folders_ParentFolderId",
                        column: x => x.ParentFolderId,
                        principalTable: "Folders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Notes_Notebooks_NotebookId",
                        column: x => x.NotebookId,
                        principalTable: "Notebooks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Notes_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913"), 0, "002957a8-8c1d-4cfc-a4eb-83f0be798ee8", "user2@lunarapp.com", true, false, null, "USER2@LUNARAPP.COM", "USER2", "AQAAAAIAAYagAAAAEPLy5n271IqOjrt3Ho4PRcUmpObK2eX4k9wP9aeMjc3mzMdpjO7dRxtsxRKhlBGYEA==", null, false, "0c43ef90-14e2-4d4e-9020-267605cdcc76", false, "User2" },
                    { new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42"), 0, "cd8ecb79-4f4f-4ca2-9dbb-fb115ddf8c39", "user1@lunarapp.com", true, false, null, "USER1@LUNARAPP.COM", "USER1", "AQAAAAIAAYagAAAAEDvWspfyDQPwIwvxtO/8CrstREtRzKTdlzJcJkhtml51PXl881GTfN67CqB3QS+1eA==", null, false, "78eeaf03-3e3f-4af8-b9f1-ec1e57d70cce", false, "User1" }
                });

            migrationBuilder.InsertData(
                table: "Notebooks",
                columns: new[] { "Id", "Description", "Title", "UserId" },
                values: new object[,]
                {
                    { new Guid("64e76744-982b-4b69-a7b1-97adb2c88939"), "Ideas for photo shoots, editing techniques, and gear recommendations.", "Photography", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("b2493638-d170-46e1-be87-2dd8c71bfd7a"), "Strategies for online marketing.", "Digital Marketing", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("bba8f1bb-cd67-414c-951f-c4fa02069a33"), "Tips and tricks for building websites.", "Web Development", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("c7fa8899-8843-4ae0-8d50-b537b7b72ec0"), "Artworks, sketches, and inspirations.", "Art Portfolio", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Name", "UserId" },
                values: new object[,]
                {
                    { new Guid("4c710bd8-a7a9-47a7-acd5-9a59fb25ac71"), "Backlog", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("558a346e-01fc-43c3-9d5f-c419f632febf"), "In Progress", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("810387bf-3876-46c9-a0c8-4bef0f905003"), "Priority", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("9551ba61-9dfd-4570-bbe1-9e84ca3c83eb"), "Important", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("9df9c706-a7ca-48de-a9e3-d3aba2d0ab78"), "Review", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("be40ffb7-35ae-4da6-b2bc-91e2ae6f0ab2"), "Completed", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("d3dfd6da-8d66-448d-b37a-84912f51988e"), "Ideas", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("da3502a0-5d57-4d40-8198-c3e36396dacc"), "Urgent", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("dcc69dcf-9f29-4eae-8b33-b1ad61f0b949"), "To-Do", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("fe656a37-42fc-4d91-aa78-4bc7ac98e2b0"), "Completed", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") }
                });

            migrationBuilder.InsertData(
                table: "Folders",
                columns: new[] { "Id", "Description", "NotebookId", "ParentFolderId", "Title", "UserId" },
                values: new object[,]
                {
                    { new Guid("0733478e-c40e-4230-a18b-cf72a9e9075b"), "Creative ideas for photo shoots.", new Guid("64e76744-982b-4b69-a7b1-97adb2c88939"), null, "Photo Shoot Ideas", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("07683ede-33ca-4ce5-94e4-1aedbd7cce54"), "Techniques for optimizing websites for search engines.", new Guid("b2493638-d170-46e1-be87-2dd8c71bfd7a"), null, "SEO Strategies", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("28380724-0d5b-417b-a4a8-49cbb3728d61"), "Best photography gear for different scenarios.", new Guid("64e76744-982b-4b69-a7b1-97adb2c88939"), null, "Gear Recommendations", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("3509f2fe-9e83-4588-8d06-4456913835b4"), "Post-processing tips for photo editing.", new Guid("64e76744-982b-4b69-a7b1-97adb2c88939"), null, "Editing Techniques", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("3b678be4-6ba8-47e8-b587-02f5eee57922"), "HTML structure and tags.", new Guid("bba8f1bb-cd67-414c-951f-c4fa02069a33"), null, "HTML Basics", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("73b21672-7ded-436d-9d21-238eac4834ee"), "Initial sketches and drawing concepts.", new Guid("c7fa8899-8843-4ae0-8d50-b537b7b72ec0"), null, "Sketches", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("7fc38d2c-36e6-4a6a-b581-20e7d5c3b6f3"), "Creating and managing successful social media campaigns.", new Guid("b2493638-d170-46e1-be87-2dd8c71bfd7a"), null, "Social Media Campaigns", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("8c124fed-1ed5-4de4-9c77-4cb58837b35a"), "JavaScript fundamentals for interactivity.", new Guid("bba8f1bb-cd67-414c-951f-c4fa02069a33"), null, "JavaScript Essentials", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("8ef17e98-cd0a-4c8a-819e-0faf9845e134"), "Building and managing email marketing lists.", new Guid("b2493638-d170-46e1-be87-2dd8c71bfd7a"), null, "Email Marketing", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("9d2247cc-c784-4b7e-bb46-453264cf771f"), "Styling techniques with CSS.", new Guid("bba8f1bb-cd67-414c-951f-c4fa02069a33"), null, "CSS Styling", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("b2d57490-28e6-46de-b945-eaac52c8039e"), "Artworks created with digital tools.", new Guid("c7fa8899-8843-4ae0-8d50-b537b7b72ec0"), null, "Digital Art", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("bcc661ec-f2dd-4383-8826-b09c4bdf9a91"), "Designing websites for all devices.", new Guid("bba8f1bb-cd67-414c-951f-c4fa02069a33"), null, "Responsive Design", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("bed7f5cd-d3e6-423c-9e7a-33a9c79f0f23"), "Artistic inspirations and reference images.", new Guid("c7fa8899-8843-4ae0-8d50-b537b7b72ec0"), null, "Inspirations", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("cd319346-1126-42da-b94c-f31027d9d644"), "Exploring popular frameworks like React and Angular.", new Guid("bba8f1bb-cd67-414c-951f-c4fa02069a33"), null, "Web Development Frameworks", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("0c28461f-a4d5-4089-83a2-a37e6a0f7b58"), "Basic concepts and setup of Angular framework.", new Guid("bba8f1bb-cd67-414c-951f-c4fa02069a33"), new Guid("cd319346-1126-42da-b94c-f31027d9d644"), "Angular Introduction", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("1818e8a2-ecaf-4b6b-a6af-760256a4719b"), "Creative photo shoot ideas for indoor settings.", new Guid("64e76744-982b-4b69-a7b1-97adb2c88939"), new Guid("0733478e-c40e-4230-a18b-cf72a9e9075b"), "Indoor Shoots", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("1eb4e1a6-701d-49f0-acf1-986462bc70b1"), "Digital portrait artworks.", new Guid("c7fa8899-8843-4ae0-8d50-b537b7b72ec0"), new Guid("b2d57490-28e6-46de-b945-eaac52c8039e"), "Portraits", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("40cdec01-fc93-48aa-a33b-d8346168ceba"), "Creating effective Facebook ads campaigns.", new Guid("b2493638-d170-46e1-be87-2dd8c71bfd7a"), new Guid("7fc38d2c-36e6-4a6a-b581-20e7d5c3b6f3"), "Facebook Ads", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("443be92e-c6cf-43d9-8b0c-5d53f6496fa3"), "Link building and other off-page SEO techniques.", new Guid("b2493638-d170-46e1-be87-2dd8c71bfd7a"), new Guid("07683ede-33ca-4ce5-94e4-1aedbd7cce54"), "Off-Page SEO", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("454281ab-4231-45e5-9c3b-d1fe9d8edcc1"), "Techniques for optimizing individual web pages.", new Guid("b2493638-d170-46e1-be87-2dd8c71bfd7a"), new Guid("07683ede-33ca-4ce5-94e4-1aedbd7cce54"), "On-Page SEO", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("4805611b-276f-43d8-887b-f32007c413cb"), "Inspirations from fantasy and sci-fi art.", new Guid("c7fa8899-8843-4ae0-8d50-b537b7b72ec0"), new Guid("bed7f5cd-d3e6-423c-9e7a-33a9c79f0f23"), "Fantasy Art", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("55145082-c6e9-4c0f-ac18-7ccbfdf356a6"), "Choosing the right lenses for different shots.", new Guid("64e76744-982b-4b69-a7b1-97adb2c88939"), new Guid("28380724-0d5b-417b-a4a8-49cbb3728d61"), "Lenses", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("696c87a5-3d7c-48dd-a3e2-31e04b226882"), "Common HTML elements and their uses.", new Guid("bba8f1bb-cd67-414c-951f-c4fa02069a33"), new Guid("3b678be4-6ba8-47e8-b587-02f5eee57922"), "HTML Elements", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("6f068948-d056-4e3d-834d-9248cd568f0c"), "Building websites with mobile-first principles.", new Guid("bba8f1bb-cd67-414c-951f-c4fa02069a33"), new Guid("bcc661ec-f2dd-4383-8826-b09c4bdf9a91"), "Mobile-First Design", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("7d292463-f14b-4fbe-894a-a23376fea131"), "Abstract digital art creations.", new Guid("c7fa8899-8843-4ae0-8d50-b537b7b72ec0"), new Guid("b2d57490-28e6-46de-b945-eaac52c8039e"), "Abstract Art", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("83c4ea28-5bcd-4aa9-9a4b-d3ad4ea05b65"), "Using CSS animations for dynamic websites.", new Guid("bba8f1bb-cd67-414c-951f-c4fa02069a33"), new Guid("9d2247cc-c784-4b7e-bb46-453264cf771f"), "CSS Animations", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("93040eb1-cf38-4c80-8d58-2ea245fd3508"), "Getting started with React framework.", new Guid("bba8f1bb-cd67-414c-951f-c4fa02069a33"), new Guid("cd319346-1126-42da-b94c-f31027d9d644"), "React Basics", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("9ac1ef98-5dca-4bcf-bebd-57e8ed06f5a2"), "Creating and using functions in JavaScript.", new Guid("bba8f1bb-cd67-414c-951f-c4fa02069a33"), new Guid("8c124fed-1ed5-4de4-9c77-4cb58837b35a"), "JavaScript Functions", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("9d6d1496-f9d6-4651-8193-2467ccba9301"), "Initial sketches of landscape designs.", new Guid("c7fa8899-8843-4ae0-8d50-b537b7b72ec0"), new Guid("73b21672-7ded-436d-9d21-238eac4834ee"), "Landscape Sketches", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("a7016214-855c-4c6e-90bc-de88d8e9e2da"), "Sketches of characters for various projects.", new Guid("c7fa8899-8843-4ae0-8d50-b537b7b72ec0"), new Guid("73b21672-7ded-436d-9d21-238eac4834ee"), "Character Designs", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("aed721b3-fa23-4cd8-bbb9-fae72cb14350"), "Basic structure of an HTML document.", new Guid("bba8f1bb-cd67-414c-951f-c4fa02069a33"), new Guid("3b678be4-6ba8-47e8-b587-02f5eee57922"), "HTML Structure", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("b51001a1-da4b-416e-84dc-bed2dff1bc58"), "Different layout techniques using CSS.", new Guid("bba8f1bb-cd67-414c-951f-c4fa02069a33"), new Guid("9d2247cc-c784-4b7e-bb46-453264cf771f"), "CSS Layouts", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("bc80f3da-b509-4880-b2aa-bd45ce2542b1"), "Techniques for building a quality email list.", new Guid("b2493638-d170-46e1-be87-2dd8c71bfd7a"), new Guid("8ef17e98-cd0a-4c8a-819e-0faf9845e134"), "List Building", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("c0f9d00f-d84c-4646-b09b-85c117f4d7ff"), "Photography references for nature-themed artworks.", new Guid("c7fa8899-8843-4ae0-8d50-b537b7b72ec0"), new Guid("bed7f5cd-d3e6-423c-9e7a-33a9c79f0f23"), "Nature Photography", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("c567ec2e-207d-4b86-89e5-60cacf4012cb"), "Best practices for designing effective email campaigns.", new Guid("b2493638-d170-46e1-be87-2dd8c71bfd7a"), new Guid("8ef17e98-cd0a-4c8a-819e-0faf9845e134"), "Email Campaign Design", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("c7ccc576-e877-4b53-b269-3e4fe37a766d"), "Post-processing tips using Lightroom.", new Guid("64e76744-982b-4b69-a7b1-97adb2c88939"), new Guid("3509f2fe-9e83-4588-8d06-4456913835b4"), "Lightroom Techniques", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("d6983107-5575-4ee6-83c2-f41349b00e05"), "Ideas for shooting in outdoor environments.", new Guid("64e76744-982b-4b69-a7b1-97adb2c88939"), new Guid("0733478e-c40e-4230-a18b-cf72a9e9075b"), "Outdoor Shoots", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("d73401ea-abcd-4c2c-b32f-25efd67159c9"), "Growing your brand on Instagram with engaging content.", new Guid("b2493638-d170-46e1-be87-2dd8c71bfd7a"), new Guid("7fc38d2c-36e6-4a6a-b581-20e7d5c3b6f3"), "Instagram Marketing", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("e0129671-97b9-496e-9bed-1b18e60d6497"), "Using media queries for responsive layouts.", new Guid("bba8f1bb-cd67-414c-951f-c4fa02069a33"), new Guid("bcc661ec-f2dd-4383-8826-b09c4bdf9a91"), "Media Queries", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("e6bf0b27-f2c6-4185-9b90-fefd5de0d482"), "Understanding variables and data types in JavaScript.", new Guid("bba8f1bb-cd67-414c-951f-c4fa02069a33"), new Guid("8c124fed-1ed5-4de4-9c77-4cb58837b35a"), "JavaScript Variables", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("e933066b-8c28-4169-adfe-84215032f9ac"), "Advanced editing using Photoshop.", new Guid("64e76744-982b-4b69-a7b1-97adb2c88939"), new Guid("3509f2fe-9e83-4588-8d06-4456913835b4"), "Photoshop Editing", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("eb5fdfa5-8262-4c9f-8c78-b8ac8339e23a"), "Recommended cameras for various types of photography.", new Guid("64e76744-982b-4b69-a7b1-97adb2c88939"), new Guid("28380724-0d5b-417b-a4a8-49cbb3728d61"), "Cameras", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") }
                });

            migrationBuilder.InsertData(
                table: "Notes",
                columns: new[] { "Id", "Body", "DateCreated", "FolderId", "LastSaved", "NotebookId", "ParentFolderId", "TagId", "Title", "UserId" },
                values: new object[,]
                {
                    { new Guid("03071a78-bb60-463d-8720-39e196605d1a"), "How to find the right keywords for SEO.", new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7151), new Guid("07683ede-33ca-4ce5-94e4-1aedbd7cce54"), new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7153), new Guid("b2493638-d170-46e1-be87-2dd8c71bfd7a"), null, new Guid("be40ffb7-35ae-4da6-b2bc-91e2ae6f0ab2"), "Keyword Research", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("103954f0-57d1-466f-bb08-6ba7f2194d85"), "Sketch of a mountain landscape with a river running through it.", new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7274), new Guid("73b21672-7ded-436d-9d21-238eac4834ee"), new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7275), new Guid("c7fa8899-8843-4ae0-8d50-b537b7b72ec0"), null, new Guid("558a346e-01fc-43c3-9d5f-c419f632febf"), "Mountain Landscape", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("12a1a380-c3a2-47c8-bd03-6de4dd2f750b"), "Ideas for capturing the beauty of nature through the lens.", new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7104), new Guid("0733478e-c40e-4230-a18b-cf72a9e9075b"), new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7105), new Guid("64e76744-982b-4b69-a7b1-97adb2c88939"), null, new Guid("9551ba61-9dfd-4570-bbe1-9e84ca3c83eb"), "Nature Photography", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("37a6cee4-d0db-4211-a3b4-140114c6cbc5"), "Tips for growing a list of engaged email subscribers.", new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7169), new Guid("8ef17e98-cd0a-4c8a-819e-0faf9845e134"), new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7170), new Guid("b2493638-d170-46e1-be87-2dd8c71bfd7a"), null, new Guid("d3dfd6da-8d66-448d-b37a-84912f51988e"), "Building an Email List", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("3d407ad9-deb5-4ce8-82c6-3da41e705b88"), "An introduction to CSS Grid layout system for building responsive designs.", new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(6977), new Guid("bcc661ec-f2dd-4383-8826-b09c4bdf9a91"), new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(6978), new Guid("bba8f1bb-cd67-414c-951f-c4fa02069a33"), null, new Guid("9df9c706-a7ca-48de-a9e3-d3aba2d0ab78"), "CSS Grid", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("5730356e-fdf8-4b8e-8517-a93bdc471f88"), "The basic structure of an HTML document with tags like <html>, <head>, <body>, etc.", new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(6887), new Guid("3b678be4-6ba8-47e8-b587-02f5eee57922"), new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(6949), new Guid("bba8f1bb-cd67-414c-951f-c4fa02069a33"), null, new Guid("9551ba61-9dfd-4570-bbe1-9e84ca3c83eb"), "HTML Structure", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("5e28a104-6632-451b-8578-1397114f6afc"), "Steps for setting up React using Create React App.", new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7072), new Guid("cd319346-1126-42da-b94c-f31027d9d644"), new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7073), new Guid("bba8f1bb-cd67-414c-951f-c4fa02069a33"), null, new Guid("fe656a37-42fc-4d91-aa78-4bc7ac98e2b0"), "React Setup", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("5fc3f138-6216-4e79-bd7e-532ee58f9adb"), "An introduction to CSS selectors for styling HTML elements.", new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(6959), new Guid("9d2247cc-c784-4b7e-bb46-453264cf771f"), new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(6960), new Guid("bba8f1bb-cd67-414c-951f-c4fa02069a33"), null, new Guid("9df9c706-a7ca-48de-a9e3-d3aba2d0ab78"), "CSS Selectors", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("74e383c9-4ca9-4ffe-85bd-c33afae68772"), "Techniques for enhancing colors in Lightroom.", new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7113), new Guid("3509f2fe-9e83-4588-8d06-4456913835b4"), new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7114), new Guid("64e76744-982b-4b69-a7b1-97adb2c88939"), null, new Guid("dcc69dcf-9f29-4eae-8b33-b1ad61f0b949"), "Color Grading in Lightroom", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("b9946f2b-405f-460a-91f6-113a8237edb3"), "A digital painting of a woman with flowing hair.", new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7284), new Guid("b2d57490-28e6-46de-b945-eaac52c8039e"), new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7285), new Guid("c7fa8899-8843-4ae0-8d50-b537b7b72ec0"), null, new Guid("810387bf-3876-46c9-a0c8-4bef0f905003"), "Digital Portrait", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("d53f3f28-816f-4f10-810a-ea1be7da24b8"), "Understanding the different types of variables in JavaScript, including var, let, and const.", new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(6967), new Guid("8c124fed-1ed5-4de4-9c77-4cb58837b35a"), new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(6969), new Guid("bba8f1bb-cd67-414c-951f-c4fa02069a33"), null, new Guid("dcc69dcf-9f29-4eae-8b33-b1ad61f0b949"), "JavaScript Variables", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("f439d93a-cc5a-464b-9f2e-aff1181ea300"), "Step-by-step guide to creating effective Facebook ads.", new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7161), new Guid("7fc38d2c-36e6-4a6a-b581-20e7d5c3b6f3"), new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7162), new Guid("b2493638-d170-46e1-be87-2dd8c71bfd7a"), null, new Guid("4c710bd8-a7a9-47a7-acd5-9a59fb25ac71"), "Creating Facebook Ads", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("f46dca2c-ff7f-450f-a7a3-2b4b4bfaa1af"), "A list of the best DSLR cameras for photography novices.", new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7121), new Guid("28380724-0d5b-417b-a4a8-49cbb3728d61"), new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7122), new Guid("64e76744-982b-4b69-a7b1-97adb2c88939"), null, new Guid("fe656a37-42fc-4d91-aa78-4bc7ac98e2b0"), "Best DSLR Cameras for Beginners", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("fe8e7876-8b2a-4c69-b2bd-bfeabb0d6d41"), "Images of forests, mountains, and rivers to inspire landscape art.", new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7292), new Guid("bed7f5cd-d3e6-423c-9e7a-33a9c79f0f23"), new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7293), new Guid("c7fa8899-8843-4ae0-8d50-b537b7b72ec0"), null, new Guid("be40ffb7-35ae-4da6-b2bc-91e2ae6f0ab2"), "Nature Art References", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("06c887d8-974f-4ca3-9593-e5906fc4b8f0"), "Creating abstract geometric shapes using digital tools.", new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7288), new Guid("1eb4e1a6-701d-49f0-acf1-986462bc70b1"), new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7289), new Guid("c7fa8899-8843-4ae0-8d50-b537b7b72ec0"), null, new Guid("d3dfd6da-8d66-448d-b37a-84912f51988e"), "Abstract Geometric Art", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("21339dbb-af0e-48f6-a2ca-617d49934d17"), "Inspirations from sci-fi concept art for a futuristic city.", new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7297), new Guid("c0f9d00f-d84c-4646-b09b-85c117f4d7ff"), new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7298), new Guid("c7fa8899-8843-4ae0-8d50-b537b7b72ec0"), null, new Guid("558a346e-01fc-43c3-9d5f-c419f632febf"), "Sci-Fi Concept Art", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("3d38093a-d616-448a-8efb-0a30bb74dd44"), "Tips for using studio lighting to enhance portraits.", new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7109), new Guid("d6983107-5575-4ee6-83c2-f41349b00e05"), new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7110), new Guid("64e76744-982b-4b69-a7b1-97adb2c88939"), null, new Guid("da3502a0-5d57-4d40-8198-c3e36396dacc"), "Studio Lighting", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("43ce3234-72da-4ae1-9bdd-845e1026b82f"), "Creating email campaigns that convert subscribers into customers.", new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7174), new Guid("bc80f3da-b509-4880-b2aa-bd45ce2542b1"), new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7175), new Guid("b2493638-d170-46e1-be87-2dd8c71bfd7a"), null, new Guid("be40ffb7-35ae-4da6-b2bc-91e2ae6f0ab2"), "Effective Email Campaigns", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("526de7eb-ae57-45c5-9b12-a9b361f7bc89"), "Techniques for optimizing your website for Google's algorithms.", new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7157), new Guid("454281ab-4231-45e5-9c3b-d1fe9d8edcc1"), new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7158), new Guid("b2493638-d170-46e1-be87-2dd8c71bfd7a"), null, new Guid("558a346e-01fc-43c3-9d5f-c419f632febf"), "Optimizing for Google", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("5f5d6ec1-68d0-4218-914f-731761647906"), "Basic Angular setup and introduction to components, services, and modules.", new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7077), new Guid("93040eb1-cf38-4c80-8d58-2ea245fd3508"), new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7078), new Guid("bba8f1bb-cd67-414c-951f-c4fa02069a33"), null, new Guid("da3502a0-5d57-4d40-8198-c3e36396dacc"), "Angular Basics", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("7bdc3d85-7c27-430b-af9c-9eb3cbff3805"), "How to use Flexbox for flexible and responsive layouts.", new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7068), new Guid("e0129671-97b9-496e-9bed-1b18e60d6497"), new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7069), new Guid("bba8f1bb-cd67-414c-951f-c4fa02069a33"), null, new Guid("9551ba61-9dfd-4570-bbe1-9e84ca3c83eb"), "Flexbox", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("82ac74b4-42c4-41e6-8b72-61707fc55b92"), "Understanding the CSS box model with padding, margin, border, and content.", new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(6964), new Guid("b51001a1-da4b-416e-84dc-bed2dff1bc58"), new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(6965), new Guid("bba8f1bb-cd67-414c-951f-c4fa02069a33"), null, new Guid("fe656a37-42fc-4d91-aa78-4bc7ac98e2b0"), "Box Model", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("9f7220e1-a76d-49be-8ade-a1b993538873"), "How to create content that engages your Instagram audience.", new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7165), new Guid("40cdec01-fc93-48aa-a33b-d8346168ceba"), new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7166), new Guid("b2493638-d170-46e1-be87-2dd8c71bfd7a"), null, new Guid("810387bf-3876-46c9-a0c8-4bef0f905003"), "Engaging Instagram Content", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("ae57c752-d0ff-4fa9-a554-d1f3be1da742"), "An overview of commonly used HTML elements like <div>, <h1>, <p>, etc.", new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(6954), new Guid("aed721b3-fa23-4cd8-bbb9-fae72cb14350"), new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(6955), new Guid("bba8f1bb-cd67-414c-951f-c4fa02069a33"), null, new Guid("dcc69dcf-9f29-4eae-8b33-b1ad61f0b949"), "Common HTML Elements", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("cbe3825d-182c-469c-92f4-cb070bbbf71e"), "A guide to selecting the best lenses for portrait shots.", new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7126), new Guid("eb5fdfa5-8262-4c9f-8c78-b8ac8339e23a"), new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7127), new Guid("64e76744-982b-4b69-a7b1-97adb2c88939"), null, new Guid("fe656a37-42fc-4d91-aa78-4bc7ac98e2b0"), "Choosing Lenses for Portrait Photography", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("d1913ba3-ca3b-4720-96b2-e44879b00e2a"), "Initial sketch of a fantasy character with magical abilities.", new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7280), new Guid("9d6d1496-f9d6-4651-8193-2467ccba9301"), new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7281), new Guid("c7fa8899-8843-4ae0-8d50-b537b7b72ec0"), null, new Guid("4c710bd8-a7a9-47a7-acd5-9a59fb25ac71"), "Fantasy Character", new Guid("3a7c43e8-a082-43fc-8c3b-199fea251913") },
                    { new Guid("e1b1acff-a86f-49a2-902b-c0d3b9fda152"), "Step-by-step guide to retouching portraits in Photoshop.", new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7117), new Guid("c7ccc576-e877-4b53-b269-3e4fe37a766d"), new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(7118), new Guid("64e76744-982b-4b69-a7b1-97adb2c88939"), null, new Guid("9df9c706-a7ca-48de-a9e3-d3aba2d0ab78"), "Retouching in Photoshop", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") },
                    { new Guid("ed841837-13d5-4fb1-a312-7f2312720cd5"), "Different types of loops in JavaScript like for, while, and do-while.", new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(6973), new Guid("e6bf0b27-f2c6-4185-9b90-fefd5de0d482"), new DateTime(2024, 12, 10, 2, 19, 47, 562, DateTimeKind.Local).AddTicks(6974), new Guid("bba8f1bb-cd67-414c-951f-c4fa02069a33"), null, new Guid("da3502a0-5d57-4d40-8198-c3e36396dacc"), "JavaScript Loops", new Guid("e3173a9a-c123-4cca-a93f-7fad5181bf42") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

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
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_NotebookId",
                table: "Folders",
                column: "NotebookId");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_ParentFolderId",
                table: "Folders",
                column: "ParentFolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_UserId",
                table: "Folders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notebooks_UserId",
                table: "Notebooks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_FolderId",
                table: "Notes",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_NotebookId",
                table: "Notes",
                column: "NotebookId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_ParentFolderId",
                table: "Notes",
                column: "ParentFolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_TagId",
                table: "Notes",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_UserId",
                table: "Notes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_UserId",
                table: "Tags",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "Notes");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Folders");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Notebooks");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
