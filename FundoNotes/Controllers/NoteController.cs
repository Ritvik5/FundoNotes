using BusinessLayer.Interface;
using CommonLayer.Model;
using Experimental.System.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RepoLayer.Context;
using RepoLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FundoNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteBusiness noteBusiness;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        private readonly FundoContext fundoContext;

        public NoteController(INoteBusiness noteBusiness,IMemoryCache memoryCache,IDistributedCache distributedCache,FundoContext fundoContext)
        {
            this.noteBusiness = noteBusiness;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
            this.fundoContext = fundoContext;
        }
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateNote(CreateNoteModel model)
        {
            try
            {
                var userIdClaim = User.FindFirst("userId");
                long userId = long.Parse(userIdClaim.Value);
                var addNote = await noteBusiness.CreateNote(model, userId);
                if(addNote != null)
                {
                    return Ok(new {success = true,message = "Your note has been Created"});
                }
                else
                {
                    return BadRequest(new { success = false, message = "Note cannot be created"});
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetNoteForParticularUser()
        {
            try
            {
                var userIdClaim = User.FindFirst("userId");
                long userId = long.Parse(userIdClaim.Value);
                List<NoteEntity> result = new List<NoteEntity>();
                result = await noteBusiness.GetAll(userId);
                if(result != null)
                {
                    return Ok(new { success = true, message = "All Notes for the user", result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Can't fetch the results" });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateNote(UpdateNoteModel model,long noteId)
        {
            try
            {
                var userIdClaim = User.FindFirst("userId");
                long userId = long.Parse(userIdClaim.Value);
                var updateNote = await noteBusiness.UpdateNote(model,noteId,userId);
                if(updateNote != null)
                {
                    return Ok(new { success = true, message = "Note has been updated", data = updateNote });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Something went wrong" });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteNote(long noteId)
        {
            try
            {
                var userIdClaim = User.FindFirst("userId");
                long userId = long.Parse(userIdClaim.Value);
                if(userId != 0 && noteId != 0) 
                {
                    await noteBusiness.DeleteNote(noteId, userId);
                    return Ok(new { success = true, message = "Note Deleted sucessfully" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Note can't be deleted" });
                }
                
                
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpPut("Archive")]
        public async Task<IActionResult> ArchiveNote(long noteId)
        {
            try
            {
                var userIdClaim = User.FindFirst("userId");
                long userId = long.Parse(userIdClaim.Value);
                var archiveNote = await noteBusiness.ArchiveNote(noteId, userId);
                if(archiveNote != null)
                {
                    return Ok(new { success = true, message = "Note has been Archived" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Something went wrong" });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpPut("Pin")]
        public async Task<IActionResult> PinNote(long noteId)
        {
            try
            {
                var userIdClaim = User.FindFirst("userId");
                long userId = long.Parse(userIdClaim.Value);
                var pinNote = await noteBusiness.PinNote(noteId, userId);
                if(pinNote != null)
                {
                    return Ok(new { success = true, message = "Note has been Pinned" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Something went wrong" });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpPut("Remainder")]
        public async Task<IActionResult> ReminderNote(long noteId)
        {
            try
            {
                var userIdClaim = User.FindFirst("userId");
                long userId = long.Parse(userIdClaim.Value);
                var reminderNote = await noteBusiness.ReminderNote(noteId,userId);
                if(reminderNote != null)
                {
                    return Ok(new { success = true, message = "Note has been marked as reminder" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Something went wrong" });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpPut("Trash")]
        public async Task<IActionResult> TrashNote(long noteId)
        {
            try
            {
                var userIdClaim = User.FindFirst("userId");
                long userId = long.Parse(userIdClaim.Value);
                var trashNote = await noteBusiness.TrashNote(noteId, userId);
                if(trashNote != null) 
                {
                    return Ok(new { success = true, message = "Note has been marked Trash" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Something went wrong" });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpPost("Colour")]
        public async Task<IActionResult> ChangeBackgroundColour(long noteId,string colour)
        {
            try
            {
                var userIdClaim = User.FindFirst("userId");
                long userId = long.Parse(userIdClaim.Value);
                var backgroundColour = await noteBusiness.BackgroundColour(noteId, userId,colour);
                if(backgroundColour != null)
                {
                    return Ok(new { success = true, message = "Background Colour has been changed." });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Something went wrong" });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpPost("image")]
        public async Task<IActionResult> ImageUpload(long noteId,IFormFile image)
        {
            try
            {
                var userIdClaim = User.FindFirst("userId");
                long userId = long.Parse(userIdClaim.Value);
                var imageUpload = await noteBusiness.UploadImage(noteId, userId, image);
                if(imageUpload != null)
                {
                    return Ok(new { success = true, message = "Image uploaded sucessfully",data=imageUpload });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Something went wrong" });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        [HttpGet("redis")]
        public async Task<IActionResult> GetAllNotesUsingRedisCache()
        {
            try
            {
                var cacheKey = "noteList";
                string serializeNoteList;
                List<NoteEntity> noteList = new List<NoteEntity>();
                byte[] redisNoteList = await distributedCache.GetAsync(cacheKey);
                if (redisNoteList != null)
                {
                    serializeNoteList = Encoding.UTF8.GetString(redisNoteList);
                    noteList = JsonConvert.DeserializeObject<List<NoteEntity>>(serializeNoteList);
                }
                else
                {
                    noteList = await fundoContext.Notes.ToListAsync();
                    serializeNoteList = JsonConvert.SerializeObject(noteList);
                    redisNoteList = Encoding.UTF8.GetBytes(serializeNoteList);
                    var options = new DistributedCacheEntryOptions()
                                 .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                                 .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    await distributedCache.SetAsync(cacheKey,redisNoteList,options);
                }
                return Ok(new { success = true, message = "All Notes", data = noteList });
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}
