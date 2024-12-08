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
                name: "Notebooks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Notebook Identifier"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Notebook title"),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 20000, nullable: true, comment: "Notebook description")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notebooks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Tag Identifier"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Tag name")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
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
                name: "Folders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Folder Identifier"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Folder title"),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 20000, nullable: true, comment: "Folder description"),
                    NotebookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identifier of a notebook"),
                    ParentFolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Identifier of a parent folder")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folders", x => x.Id);
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Note Identifier"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Note title"),
                    Body = table.Column<string>(type: "nvarchar(max)", maxLength: 20000, nullable: true, comment: "Note body"),
                    NotebookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Identifier of a notebook"),
                    ParentFolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FolderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Identifier of a folder"),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "Identifier of a tag"),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "The date the note was created on"),
                    LastSaved = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "The date the note was last saved on")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notes_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Notebooks",
                columns: new[] { "Id", "Description", "Title" },
                values: new object[,]
                {
                    { new Guid("92bda610-1b12-4a0a-b12b-5e11aa47c008"), "A notebook for stories, poetry, and creative writing ideas.", "Creative Writing" },
                    { new Guid("a5b9e3e5-c41a-40c2-9068-b4ff72fc585b"), "A place for goals, self-improvement tips, and productivity hacks.", "Personal Development" },
                    { new Guid("c1052546-dad1-4569-aa20-4d31db57af81"), "Notes on video editing, techniques, and tutorials.", "Video Editing" },
                    { new Guid("e36ef31b-16d9-40d6-a336-0dadb73ee706"), "A notebook for all things software development.", "Software Engineering" },
                    { new Guid("e3e6aa3b-c280-4005-8fb2-14c4cdcf6425"), "For all music production-related notes.", "Music Production" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("2626d426-58ee-4a83-9b61-826c7bd12b37"), "Not Urgent" },
                    { new Guid("67d6c49b-f33f-4e50-87bd-fc76808f7bb9"), "Urgent" },
                    { new Guid("d70e52df-9509-4651-89b2-3f5c83553399"), "Important" }
                });

            migrationBuilder.InsertData(
                table: "Folders",
                columns: new[] { "Id", "Description", "NotebookId", "ParentFolderId", "Title" },
                values: new object[,]
                {
                    { new Guid("1f5c5793-2e19-4f9d-adc8-b1c750666336"), "Track personal development goals.", new Guid("a5b9e3e5-c41a-40c2-9068-b4ff72fc585b"), null, "Goals" },
                    { new Guid("617fd59e-c7f6-4e90-a9ca-d9b6cfc61bce"), "Links to useful software engineering resources.", new Guid("e36ef31b-16d9-40d6-a336-0dadb73ee706"), null, "Resources" },
                    { new Guid("9005ce81-b076-4a8f-a56e-c9ef43e11021"), "Ideas for future stories or poems.", new Guid("92bda610-1b12-4a0a-b12b-5e11aa47c008"), null, "Ideas" },
                    { new Guid("9a2143ce-86da-4450-8c21-aca5ca0becfb"), "Tutorials on video editing techniques.", new Guid("c1052546-dad1-4569-aa20-4d31db57af81"), null, "Techniques" },
                    { new Guid("a4009323-ffde-4dde-898a-c63a1a31c000"), "Quick access to reusable code.", new Guid("e36ef31b-16d9-40d6-a336-0dadb73ee706"), null, "Code Snippets" },
                    { new Guid("d9c92c27-b608-48e2-bb8e-790e333020f2"), "A collection of music samples for production.", new Guid("e3e6aa3b-c280-4005-8fb2-14c4cdcf6425"), null, "Samples" }
                });

            migrationBuilder.InsertData(
                table: "Notes",
                columns: new[] { "Id", "Body", "DateCreated", "FolderId", "LastSaved", "NotebookId", "ParentFolderId", "TagId", "Title" },
                values: new object[,]
                {
                    { new Guid("10966e78-2493-4ef6-96f0-0be25980bc15"), "A step-by-step guide to an effective morning routine to start the day right.", new DateTime(2024, 12, 8, 11, 21, 31, 965, DateTimeKind.Local).AddTicks(5093), new Guid("1f5c5793-2e19-4f9d-adc8-b1c750666336"), new DateTime(2024, 12, 8, 11, 21, 31, 965, DateTimeKind.Local).AddTicks(5094), new Guid("a5b9e3e5-c41a-40c2-9068-b4ff72fc585b"), null, null, "Morning Routine" },
                    { new Guid("28702685-e6f8-4829-a937-a3b41d722c77"), "A guide to color grading for video editors using different software.", new DateTime(2024, 12, 8, 11, 21, 31, 965, DateTimeKind.Local).AddTicks(5088), new Guid("9a2143ce-86da-4450-8c21-aca5ca0becfb"), new DateTime(2024, 12, 8, 11, 21, 31, 965, DateTimeKind.Local).AddTicks(5089), new Guid("c1052546-dad1-4569-aa20-4d31db57af81"), null, null, "Color Grading Techniques" },
                    { new Guid("508fe25d-73f5-41be-9bcf-6cf8777d57b5"), "Techniques for creating deep, multi-dimensional characters in fiction.", new DateTime(2024, 12, 8, 11, 21, 31, 965, DateTimeKind.Local).AddTicks(5097), new Guid("9005ce81-b076-4a8f-a56e-c9ef43e11021"), new DateTime(2024, 12, 8, 11, 21, 31, 965, DateTimeKind.Local).AddTicks(5098), new Guid("92bda610-1b12-4a0a-b12b-5e11aa47c008"), null, null, "Character Development" },
                    { new Guid("6416fd93-8446-4359-bf1b-06bbe9935a47"), "Notes on various software design patterns, including Singleton, Factory, and Observer.", new DateTime(2024, 12, 8, 11, 21, 31, 965, DateTimeKind.Local).AddTicks(5025), new Guid("a4009323-ffde-4dde-898a-c63a1a31c000"), new DateTime(2024, 12, 8, 11, 21, 31, 965, DateTimeKind.Local).AddTicks(5080), new Guid("e36ef31b-16d9-40d6-a336-0dadb73ee706"), null, null, "Design Patterns" },
                    { new Guid("92a20920-197f-4f8e-8e71-1679ef7317a0"), "Techniques for mixing tracks in a DAW, with focus on EQ and compression.", new DateTime(2024, 12, 8, 11, 21, 31, 965, DateTimeKind.Local).AddTicks(5084), new Guid("d9c92c27-b608-48e2-bb8e-790e333020f2"), new DateTime(2024, 12, 8, 11, 21, 31, 965, DateTimeKind.Local).AddTicks(5085), new Guid("e3e6aa3b-c280-4005-8fb2-14c4cdcf6425"), null, null, "Mixing Tips" }
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
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Folders");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Notebooks");
        }
    }
}
