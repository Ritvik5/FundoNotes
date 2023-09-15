using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using RepoLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepoLayer.Interface
{
    public interface INoteRepo
    {
        Task<NoteEntity> CreateNote(CreateNoteModel model, long userId);
        Task<List<NoteEntity>> GetAll(long userId);
        Task<NoteEntity> UpdateNote(UpdateNoteModel model, long noteId, long userId);
        Task<bool> DeleteNote(long noteId, long userId);
        Task<NoteEntity> ArchiveNote(long noteId, long userId);
        Task<NoteEntity> PinNote(long noteId, long userId);
        Task<NoteEntity> ReminderNote(long noteId, long userId);
        Task<NoteEntity> TrashNote(long noteId, long userId);
        Task<NoteEntity> BackgroundColour(long noteId, long userId, string colour);
        Task<string> UploadImage(long noteId, long userId, IFormFile image);
    }
}
