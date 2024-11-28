using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LunarApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
