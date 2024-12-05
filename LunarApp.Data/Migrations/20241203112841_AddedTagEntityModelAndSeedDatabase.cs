using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LunarApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedTagEntityModelAndSeedDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: new Guid("84aebc41-d5cc-47ab-8f11-97855f3c2867"));

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: new Guid("41f96f39-2a4c-4f53-a060-a04db665eccb"));

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: new Guid("8adb0ab3-de9d-4306-9df7-98ab07bc97d8"));

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: new Guid("94705b4b-df53-4cad-9609-866f87dc28a1"));

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: new Guid("cca1d0ee-e770-4a11-a831-39d6e0552ccd"));

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: new Guid("ef1dd281-ff79-4a8a-bfbc-a4182a8e7ed1"));

            migrationBuilder.DeleteData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: new Guid("22c0b5d6-7d91-4a3d-9472-31871d581104"));

            migrationBuilder.DeleteData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: new Guid("2bbbf65b-7b94-477e-89c8-0efacf17955c"));

            migrationBuilder.DeleteData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: new Guid("46cd0253-1475-4421-a3a2-7bbba61af78e"));

            migrationBuilder.DeleteData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: new Guid("5f07bccd-9bbf-470e-8b65-d07695f5e593"));

            migrationBuilder.DeleteData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: new Guid("79f0d3f6-5b36-4f41-946c-6b8cab49decd"));

            migrationBuilder.DeleteData(
                table: "Notebooks",
                keyColumn: "Id",
                keyValue: new Guid("1ed79ae8-b8ae-471f-9654-575a6d8ce605"));

            migrationBuilder.DeleteData(
                table: "Notebooks",
                keyColumn: "Id",
                keyValue: new Guid("4d42eb54-4281-44e7-ac9c-cebaa8fc3599"));

            migrationBuilder.DeleteData(
                table: "Notebooks",
                keyColumn: "Id",
                keyValue: new Guid("720bd9c9-fbf2-461d-bd6b-64ed03eba3e1"));

            migrationBuilder.DeleteData(
                table: "Notebooks",
                keyColumn: "Id",
                keyValue: new Guid("d8ce68d6-f596-4670-9dd4-73c707a902cc"));

            migrationBuilder.DeleteData(
                table: "Notebooks",
                keyColumn: "Id",
                keyValue: new Guid("e5904304-a51b-4b34-a302-20f2abc98c4f"));

            migrationBuilder.AddColumn<Guid>(
                name: "TagId",
                table: "Notes",
                type: "uniqueidentifier",
                nullable: true,
                comment: "Identifier of a tag");

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

            migrationBuilder.InsertData(
                table: "Notebooks",
                columns: new[] { "Id", "Description", "Title" },
                values: new object[,]
                {
                    { new Guid("17d0d13b-1c3e-4e29-a85e-ebd38cec944d"), "A notebook for stories, poetry, and creative writing ideas.", "Creative Writing" },
                    { new Guid("4545e175-6a16-4c7d-8f1f-b87c2846e1aa"), "Notes on video editing, techniques, and tutorials.", "Video Editing" },
                    { new Guid("68140580-97ec-4543-bda7-5c899609a098"), "A notebook for all things software development.", "Software Engineering" },
                    { new Guid("6f45feff-2f73-495d-9d09-0ec08a8b7ac8"), "A place for goals, self-improvement tips, and productivity hacks.", "Personal Development" },
                    { new Guid("a566461d-ebd6-4f09-9ba1-a979b2cbb6fd"), "For all music production-related notes.", "Music Production" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("62f48b86-54b4-4c14-a81f-e6a734564921"), "Urgent" },
                    { new Guid("c21469f5-0620-4d87-9344-7779d15ada28"), "Important" },
                    { new Guid("e6bd5210-227a-4646-9a3f-a8f79098bd7c"), "Not Urgent" }
                });

            migrationBuilder.InsertData(
                table: "Folders",
                columns: new[] { "Id", "Description", "NotebookId", "ParentFolderId", "Title" },
                values: new object[,]
                {
                    { new Guid("0fc58ea1-fb37-47bd-821c-36073702f684"), "Quick access to reusable code.", new Guid("68140580-97ec-4543-bda7-5c899609a098"), null, "Code Snippets" },
                    { new Guid("56044a58-7c4d-4248-b3b7-84dc0d94ab3f"), "Track personal development goals.", new Guid("6f45feff-2f73-495d-9d09-0ec08a8b7ac8"), null, "Goals" },
                    { new Guid("bc909db1-0907-4241-a15e-85d8eb53cefc"), "Links to useful software engineering resources.", new Guid("68140580-97ec-4543-bda7-5c899609a098"), null, "Resources" },
                    { new Guid("c8b1a27b-df7b-48c0-95dc-cdc811f57e41"), "Tutorials on video editing techniques.", new Guid("4545e175-6a16-4c7d-8f1f-b87c2846e1aa"), null, "Techniques" },
                    { new Guid("e8f5eafd-ad47-4080-8ec4-81473206e201"), "Ideas for future stories or poems.", new Guid("17d0d13b-1c3e-4e29-a85e-ebd38cec944d"), null, "Ideas" },
                    { new Guid("faf44aa2-1e98-48ff-80d5-847b7b6e8b64"), "A collection of music samples for production.", new Guid("a566461d-ebd6-4f09-9ba1-a979b2cbb6fd"), null, "Samples" }
                });

            migrationBuilder.InsertData(
                table: "Notes",
                columns: new[] { "Id", "Body", "DateCreated", "FolderId", "LastSaved", "NotebookId", "TagId", "Title" },
                values: new object[,]
                {
                    { new Guid("0cd87114-a7dd-4f7e-86c0-94186ddbddc5"), "A step-by-step guide to an effective morning routine to start the day right.", new DateTime(2024, 12, 3, 13, 28, 41, 71, DateTimeKind.Local).AddTicks(9141), new Guid("56044a58-7c4d-4248-b3b7-84dc0d94ab3f"), new DateTime(2024, 12, 3, 13, 28, 41, 71, DateTimeKind.Local).AddTicks(9142), new Guid("6f45feff-2f73-495d-9d09-0ec08a8b7ac8"), null, "Morning Routine" },
                    { new Guid("6f510028-6635-46bb-98e5-40533baaf488"), "A guide to color grading for video editors using different software.", new DateTime(2024, 12, 3, 13, 28, 41, 71, DateTimeKind.Local).AddTicks(9137), new Guid("c8b1a27b-df7b-48c0-95dc-cdc811f57e41"), new DateTime(2024, 12, 3, 13, 28, 41, 71, DateTimeKind.Local).AddTicks(9138), new Guid("4545e175-6a16-4c7d-8f1f-b87c2846e1aa"), null, "Color Grading Techniques" },
                    { new Guid("7f5a5c1b-1a48-47d3-8f55-c0bf0ca8f396"), "Techniques for mixing tracks in a DAW, with focus on EQ and compression.", new DateTime(2024, 12, 3, 13, 28, 41, 71, DateTimeKind.Local).AddTicks(9133), new Guid("faf44aa2-1e98-48ff-80d5-847b7b6e8b64"), new DateTime(2024, 12, 3, 13, 28, 41, 71, DateTimeKind.Local).AddTicks(9134), new Guid("a566461d-ebd6-4f09-9ba1-a979b2cbb6fd"), null, "Mixing Tips" },
                    { new Guid("94934d02-525a-47b5-8f0c-c1fc63bd738d"), "Techniques for creating deep, multi-dimensional characters in fiction.", new DateTime(2024, 12, 3, 13, 28, 41, 71, DateTimeKind.Local).AddTicks(9147), new Guid("e8f5eafd-ad47-4080-8ec4-81473206e201"), new DateTime(2024, 12, 3, 13, 28, 41, 71, DateTimeKind.Local).AddTicks(9148), new Guid("17d0d13b-1c3e-4e29-a85e-ebd38cec944d"), null, "Character Development" },
                    { new Guid("b306fdc2-2943-41cb-b0fc-6b1f2842b6e9"), "Notes on various software design patterns, including Singleton, Factory, and Observer.", new DateTime(2024, 12, 3, 13, 28, 41, 71, DateTimeKind.Local).AddTicks(9085), new Guid("0fc58ea1-fb37-47bd-821c-36073702f684"), new DateTime(2024, 12, 3, 13, 28, 41, 71, DateTimeKind.Local).AddTicks(9129), new Guid("68140580-97ec-4543-bda7-5c899609a098"), null, "Design Patterns" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notes_TagId",
                table: "Notes",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Tags_TagId",
                table: "Notes",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Tags_TagId",
                table: "Notes");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Notes_TagId",
                table: "Notes");

            migrationBuilder.DeleteData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: new Guid("bc909db1-0907-4241-a15e-85d8eb53cefc"));

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: new Guid("0cd87114-a7dd-4f7e-86c0-94186ddbddc5"));

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: new Guid("6f510028-6635-46bb-98e5-40533baaf488"));

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: new Guid("7f5a5c1b-1a48-47d3-8f55-c0bf0ca8f396"));

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: new Guid("94934d02-525a-47b5-8f0c-c1fc63bd738d"));

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: new Guid("b306fdc2-2943-41cb-b0fc-6b1f2842b6e9"));

            migrationBuilder.DeleteData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: new Guid("0fc58ea1-fb37-47bd-821c-36073702f684"));

            migrationBuilder.DeleteData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: new Guid("56044a58-7c4d-4248-b3b7-84dc0d94ab3f"));

            migrationBuilder.DeleteData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: new Guid("c8b1a27b-df7b-48c0-95dc-cdc811f57e41"));

            migrationBuilder.DeleteData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: new Guid("e8f5eafd-ad47-4080-8ec4-81473206e201"));

            migrationBuilder.DeleteData(
                table: "Folders",
                keyColumn: "Id",
                keyValue: new Guid("faf44aa2-1e98-48ff-80d5-847b7b6e8b64"));

            migrationBuilder.DeleteData(
                table: "Notebooks",
                keyColumn: "Id",
                keyValue: new Guid("17d0d13b-1c3e-4e29-a85e-ebd38cec944d"));

            migrationBuilder.DeleteData(
                table: "Notebooks",
                keyColumn: "Id",
                keyValue: new Guid("4545e175-6a16-4c7d-8f1f-b87c2846e1aa"));

            migrationBuilder.DeleteData(
                table: "Notebooks",
                keyColumn: "Id",
                keyValue: new Guid("68140580-97ec-4543-bda7-5c899609a098"));

            migrationBuilder.DeleteData(
                table: "Notebooks",
                keyColumn: "Id",
                keyValue: new Guid("6f45feff-2f73-495d-9d09-0ec08a8b7ac8"));

            migrationBuilder.DeleteData(
                table: "Notebooks",
                keyColumn: "Id",
                keyValue: new Guid("a566461d-ebd6-4f09-9ba1-a979b2cbb6fd"));

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "Notes");

            migrationBuilder.InsertData(
                table: "Notebooks",
                columns: new[] { "Id", "Description", "Title" },
                values: new object[,]
                {
                    { new Guid("1ed79ae8-b8ae-471f-9654-575a6d8ce605"), "A place for goals, self-improvement tips, and productivity hacks.", "Personal Development" },
                    { new Guid("4d42eb54-4281-44e7-ac9c-cebaa8fc3599"), "Notes on video editing, techniques, and tutorials.", "Video Editing" },
                    { new Guid("720bd9c9-fbf2-461d-bd6b-64ed03eba3e1"), "A notebook for all things software development.", "Software Engineering" },
                    { new Guid("d8ce68d6-f596-4670-9dd4-73c707a902cc"), "For all music production-related notes.", "Music Production" },
                    { new Guid("e5904304-a51b-4b34-a302-20f2abc98c4f"), "A notebook for stories, poetry, and creative writing ideas.", "Creative Writing" }
                });

            migrationBuilder.InsertData(
                table: "Folders",
                columns: new[] { "Id", "Description", "NotebookId", "ParentFolderId", "Title" },
                values: new object[,]
                {
                    { new Guid("22c0b5d6-7d91-4a3d-9472-31871d581104"), "Tutorials on video editing techniques.", new Guid("4d42eb54-4281-44e7-ac9c-cebaa8fc3599"), null, "Techniques" },
                    { new Guid("2bbbf65b-7b94-477e-89c8-0efacf17955c"), "A collection of music samples for production.", new Guid("d8ce68d6-f596-4670-9dd4-73c707a902cc"), null, "Samples" },
                    { new Guid("46cd0253-1475-4421-a3a2-7bbba61af78e"), "Track personal development goals.", new Guid("1ed79ae8-b8ae-471f-9654-575a6d8ce605"), null, "Goals" },
                    { new Guid("5f07bccd-9bbf-470e-8b65-d07695f5e593"), "Ideas for future stories or poems.", new Guid("e5904304-a51b-4b34-a302-20f2abc98c4f"), null, "Ideas" },
                    { new Guid("79f0d3f6-5b36-4f41-946c-6b8cab49decd"), "Quick access to reusable code.", new Guid("720bd9c9-fbf2-461d-bd6b-64ed03eba3e1"), null, "Code Snippets" },
                    { new Guid("84aebc41-d5cc-47ab-8f11-97855f3c2867"), "Links to useful software engineering resources.", new Guid("720bd9c9-fbf2-461d-bd6b-64ed03eba3e1"), null, "Resources" }
                });

            migrationBuilder.InsertData(
                table: "Notes",
                columns: new[] { "Id", "Body", "DateCreated", "FolderId", "LastSaved", "NotebookId", "Title" },
                values: new object[,]
                {
                    { new Guid("41f96f39-2a4c-4f53-a060-a04db665eccb"), "A step-by-step guide to an effective morning routine to start the day right.", new DateTime(2024, 11, 27, 20, 37, 28, 186, DateTimeKind.Local).AddTicks(6878), new Guid("46cd0253-1475-4421-a3a2-7bbba61af78e"), new DateTime(2024, 11, 27, 20, 37, 28, 186, DateTimeKind.Local).AddTicks(6879), new Guid("1ed79ae8-b8ae-471f-9654-575a6d8ce605"), "Morning Routine" },
                    { new Guid("8adb0ab3-de9d-4306-9df7-98ab07bc97d8"), "Techniques for mixing tracks in a DAW, with focus on EQ and compression.", new DateTime(2024, 11, 27, 20, 37, 28, 186, DateTimeKind.Local).AddTicks(6869), new Guid("2bbbf65b-7b94-477e-89c8-0efacf17955c"), new DateTime(2024, 11, 27, 20, 37, 28, 186, DateTimeKind.Local).AddTicks(6871), new Guid("d8ce68d6-f596-4670-9dd4-73c707a902cc"), "Mixing Tips" },
                    { new Guid("94705b4b-df53-4cad-9609-866f87dc28a1"), "Techniques for creating deep, multi-dimensional characters in fiction.", new DateTime(2024, 11, 27, 20, 37, 28, 186, DateTimeKind.Local).AddTicks(6881), new Guid("5f07bccd-9bbf-470e-8b65-d07695f5e593"), new DateTime(2024, 11, 27, 20, 37, 28, 186, DateTimeKind.Local).AddTicks(6883), new Guid("e5904304-a51b-4b34-a302-20f2abc98c4f"), "Character Development" },
                    { new Guid("cca1d0ee-e770-4a11-a831-39d6e0552ccd"), "A guide to color grading for video editors using different software.", new DateTime(2024, 11, 27, 20, 37, 28, 186, DateTimeKind.Local).AddTicks(6874), new Guid("22c0b5d6-7d91-4a3d-9472-31871d581104"), new DateTime(2024, 11, 27, 20, 37, 28, 186, DateTimeKind.Local).AddTicks(6875), new Guid("4d42eb54-4281-44e7-ac9c-cebaa8fc3599"), "Color Grading Techniques" },
                    { new Guid("ef1dd281-ff79-4a8a-bfbc-a4182a8e7ed1"), "Notes on various software design patterns, including Singleton, Factory, and Observer.", new DateTime(2024, 11, 27, 20, 37, 28, 186, DateTimeKind.Local).AddTicks(6814), new Guid("79f0d3f6-5b36-4f41-946c-6b8cab49decd"), new DateTime(2024, 11, 27, 20, 37, 28, 186, DateTimeKind.Local).AddTicks(6864), new Guid("720bd9c9-fbf2-461d-bd6b-64ed03eba3e1"), "Design Patterns" }
                });
        }
    }
}
