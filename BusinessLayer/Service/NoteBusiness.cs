using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using RepoLayer.Entities;
using RepoLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class NoteBusiness : INoteBusiness
    {
        private readonly INoteRepo noteRepo;

        public NoteBusiness(INoteRepo noteRepo)
        {
            this.noteRepo = noteRepo;
        }

        public async Task<NoteEntity> CreateNote(CreateNoteModel model, long userId)
        {
            try
            {
                return await noteRepo.CreateNote(model, userId);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<NoteEntity>> GetAll(long userId)
        {
            try
            {
                return await noteRepo.GetAll(userId);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<NoteEntity> UpdateNote(UpdateNoteModel model, long noteId, long userId)
        {
            try
            {
                return await noteRepo.UpdateNote(model, noteId, userId);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DeleteNote(long noteId, long userId)
        {
            try
            {
                return await noteRepo.DeleteNote(noteId, userId);   
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<NoteEntity> ArchiveNote(long noteId, long userId)
        {
            try
            {
                return await noteRepo.ArchiveNote(noteId, userId);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<NoteEntity> PinNote(long noteId, long userId)
        {
            try
            {
                return await noteRepo.PinNote(noteId, userId);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<NoteEntity> ReminderNote(long noteId, long userId)
        {
            try
            {
                return await noteRepo.ReminderNote(noteId, userId);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<NoteEntity> TrashNote(long noteId, long userId)
        {
            try
            {
                return await noteRepo.TrashNote(noteId, userId);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<NoteEntity> BackgroundColour(long noteId, long userId, string colour)
        {
            try
            {
                return await noteRepo.BackgroundColour(noteId, userId, colour);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<string> UploadImage(long noteId, long userId, IFormFile image)
        {
            try
            {
                return await noteRepo.UploadImage(noteId, userId, image);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
