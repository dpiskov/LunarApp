﻿using LunarApp.Data.Models;
using LunarApp.Data.Repository.Interfaces;
using LunarApp.Services.Data.Interfaces;
using LunarApp.Web.ViewModels.Folder;
using LunarApp.Web.ViewModels.Note;
using Microsoft.EntityFrameworkCore;

namespace LunarApp.Services.Data
{
    public class BaseService(IRepository<Tag, Guid> tagRepository, IRepository<Note, Guid> noteRepository)
        : IBaseService
    {
        public Task<FolderNotesViewModel> GetFilteredNotesAsyncByNotebookId(Guid notebookId, Guid? parentFolderId, Guid? folderId, string? searchQuery,
            string? tagFilter)
        {
            throw new NotImplementedException();
        }

        public async Task<FolderNotesViewModel> GetFilteredNotesAsync(string? searchQuery, string? tagFilter)
        {
            Guid? tagId = null;

            // Retrieve tagId based on the tag name
            if (!string.IsNullOrWhiteSpace(tagFilter))
            {
                Guid tag = await tagRepository.GetAllAttached()
                    .Where(t => t.Name == tagFilter)
                    .Select(t => t.Id)
                    .FirstOrDefaultAsync();

                if (tag != Guid.Empty)
                {
                    tagId = tag;
                }
            }

            IQueryable<Note> query = noteRepository
                .GetAllAttached();

            // Fetch notes from the database
            List<Note> notesInMemory = await query.ToListAsync();

            // Apply search query filter
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                notesInMemory = notesInMemory
                    .Where(n => n.Title.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Apply tag filter
            if (tagId.HasValue)
            {
                notesInMemory = notesInMemory
                    .Where(n => n.TagId == tagId)
                    .ToList();
            }

            // Map the notes to the desired ViewModel
            List<NoteInfoViewModel> notes = notesInMemory.Select(n => new NoteInfoViewModel
            {
                Id = n.Id,
                Title = n.Title,
                NotebookId = n.NotebookId,
                ParentFolderId = n.ParentFolderId,
                FolderId = n.FolderId,
                TagId = n.TagId
            })
                .OrderBy(n => n.Title)
                .ToList();

            return new FolderNotesViewModel
            {
                Folders = new List<FolderInfoViewModel>(),  // No folders, only notes
                Notes = notes
            };
        }

        public async Task<IEnumerable<string>> GetAllTagsAsync()
        {
            IEnumerable<string> allTags = await tagRepository
                .GetAllAttached()
                .Select(t => t.Name)
                .Distinct()
                .ToArrayAsync();

            return allTags;
        }
    }
}