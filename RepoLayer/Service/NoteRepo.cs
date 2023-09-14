using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RepoLayer.Context;
using RepoLayer.Entities;
using RepoLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RepoLayer.Service
{
    public class NoteRepo : INoteRepo
    {
        private readonly FundoContext fundoContext;
        private readonly IConfiguration configuration;

        public NoteRepo(FundoContext fundoContext, IConfiguration configuration)
        {
            this.fundoContext = fundoContext;
            this.configuration = configuration;
        }
        /// <summary>
        /// Create New Note
        /// </summary>
        /// <param name="model"> New Note Info</param>
        /// <param name="userId"> User Id </param>
        /// <returns> Note Info </returns>
        public async Task<NoteEntity> CreateNote(CreateNoteModel model,long userId)
        {
            try
            {
                var user = await fundoContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
                
                NoteEntity note = new NoteEntity
                {
                    User = user
                };
                
                note.Title = model.Title;
                note.Description = model.Description;
                note.BGColor = model.BGColor;
                note.IsArchive = false;
                note.IsReminder = false;
                note.IsPin = false;
                note.IsTrash = false;
                note.CreatedTime = DateTime.Now;
                note.UpdatedTime = DateTime.Now;
                
                fundoContext.Notes.Add(note);
                await fundoContext.SaveChangesAsync();
                
                if (note != null)
                {
                    return note;
                }
                else
                {
                    return null;
                }
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
                return await fundoContext.Notes.Where(u => u.UserId == userId).Include(u=>u.User).ToListAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<NoteEntity> UpdateNote(UpdateNoteModel model,long noteId,long userId)
        {
            try
            {
                var result = fundoContext.Notes.FirstOrDefault(u => u.NoteId == noteId && u.UserId == userId);
                if(result != null)
                {
                    result.Title = model.Title;
                    result.Description = model.Description;
                    result.BGColor = model.BGColor;
                    result.IsArchive = model.IsArchive;
                    result.IsPin = model.IsPin;
                    result.IsReminder = model.IsReminder;
                    result.IsTrash = model.IsTrash;
                    result.UpdatedTime = DateTime.Now;
                    await fundoContext.SaveChangesAsync();

                    return await fundoContext.Notes.Where(n => n.NoteId == noteId).FirstOrDefaultAsync();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task DeleteNote(long noteId,long userId)
        {
            try
            {
                var user = fundoContext.Notes.FirstOrDefault(u => u.NoteId == noteId && u.UserId == userId);
                fundoContext.Notes.Remove(user);
                await fundoContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<NoteEntity> ArchiveNote(long noteId,long userId)
        {
            try
            {
                var user = fundoContext.Notes.FirstOrDefault(u => u.NoteId == noteId && u.UserId == userId);
                if(user != null)
                {
                    if(user.IsArchive == false)
                    {
                        user.IsArchive = true;
                    }
                    else
                    {
                        user.IsArchive = false;
                    }
                    await fundoContext.SaveChangesAsync();
                    return await fundoContext.Notes.Where(n => n.NoteId == noteId).FirstOrDefaultAsync();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<NoteEntity> PinNote(long noteId,long userId)
        {
            try
            {
                var user = fundoContext.Notes.FirstOrDefault(u => u.NoteId == noteId && u.UserId == userId);
                if(user != null)
                {
                    if(user.IsPin == false)
                    {
                        user.IsPin = true;
                    }
                    else
                    {
                        user.IsPin = false;
                    }
                }
                else
                {
                    return null;
                }
                await fundoContext.SaveChangesAsync();
                return await fundoContext.Notes.Where(n => n.NoteId==noteId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<NoteEntity> ReminderNote(long noteId,long userId)
        {
            try
            {
                var user = fundoContext.Notes.FirstOrDefault(u => u.NoteId == noteId && u.UserId == userId);
                if (user != null)
                {
                    if (user.IsReminder == false)
                    {
                        user.IsReminder = true;
                    }
                    else
                    {
                        user.IsReminder = false;
                    }
                    await fundoContext.SaveChangesAsync();
                    return await fundoContext.Notes.Where(u => u.NoteId == noteId).FirstOrDefaultAsync();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<NoteEntity> TrashNote(long noteId,long userId)
        {
            try
            {
                var user = fundoContext.Notes.FirstOrDefault(u => u.NoteId == noteId && u.UserId == userId);
                if (user != null)
                {
                    if(user.IsTrash == false)
                    {
                        user.IsTrash = true;
                    }
                    else
                    {
                        user.IsTrash = false;
                    }
                }
                else
                {
                    return null;
                }
                await fundoContext.SaveChangesAsync();
                return await fundoContext.Notes.Where(n => n.NoteId == noteId).FirstOrDefaultAsync(); 
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<NoteEntity> BackgroundColour(long noteId,long userId,string colour)
        {
            try
            {
                var user = fundoContext.Notes.FirstOrDefault(u => u.NoteId == noteId && u.UserId == userId);
                if (user != null)
                {
                    user.BGColor = colour;
                }
                else
                {
                    return null;
                }
                await fundoContext.SaveChangesAsync();
                return await fundoContext.Notes.Where(n => n.NoteId==noteId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<string> UploadImage(long noteId,long userId, IFormFile image)
        {
            try
            {
                var user = await fundoContext.Notes.FirstOrDefaultAsync(u => u.NoteId == noteId && u.UserId == userId);
                if (user != null)
                {
                    Account cloudinaryAccount = new Account(
                    configuration["Cloudinary:CloudName"],
                    configuration["Cloudinary:ApiKey"],
                    configuration["Cloudinary:ApiSecret"]);

                    Cloudinary cloudinary = new Cloudinary(cloudinaryAccount);
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(image.FileName, image.OpenReadStream()),
                        Transformation = new Transformation().Crop("fit").Gravity("face")
                    };
                    var uploadResult = await cloudinary.UploadAsync(uploadParams);
                    string secureurl = uploadResult.SecureUrl.ToString();
                    return "Image Uploaded Successfully.Url for image: "+ secureurl;
                }
                else
                {
                    return "Image can't uploaded";
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}

