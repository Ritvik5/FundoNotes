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
    /// <summary>
    /// Note Controller
    /// </summary>
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
        /// <summary>
        /// Create New Note
        /// </summary>
        /// <param name="model"> New Note info</param>
        /// <returns> SMD(Status,Message,Data(Created Note info)) </returns>
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
                    return Ok(new {success = true,message = "Your note has been Created", data = addNote});
                }
                else
                {
                    return BadRequest(new { success = false, message = "Note cannot be created"});
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Getting note for particular User
        /// </summary>
        /// <returns> SMD(Status,Message,Data(Note info)) </returns>
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
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Update Note 
        /// </summary>
        /// <param name="model"> Updated Note info </param>
        /// <param name="noteId"> Note Id </param>
        /// <returns> SMD(Status,Message,Data(Updated Note info)) </returns>
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
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Remove a Note
        /// </summary>
        /// <param name="noteId"> Note Id </param>
        /// <returns> SMD(Status,Message,Data) Format </returns>
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
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// To Mark Note as Archive
        /// </summary>
        /// <param name="noteId"> Note Id </param>
        /// <returns> SMD(Status, Message , Data(Note Info) </returns>
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
                    return Ok(new { success = true, message = "Note has been Archived" ,data = archiveNote});
                }
                else
                {
                    return BadRequest(new { success = false, message = "Something went wrong" });
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// To Pin a Note
        /// </summary>
        /// <param name="noteId"> Note Id </param>
        /// <returns> SMD(Status, Message , Data(Note Info) </returns>
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
                    return Ok(new { success = true, message = "Note has been Pinned" , data = pinNote});
                }
                else
                {
                    return BadRequest(new { success = false, message = "Something went wrong"});
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// To Mark Note as Reminder
        /// </summary>
        /// <param name="noteId"> Note Id </param>
        /// <returns> SMD(Status, Message , Data(Note Info) </returns>
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
                    return Ok(new { success = true, message = "Note has been marked as reminder",data = reminderNote });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Something went wrong" });
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// To Mark Note as Trash
        /// </summary>
        /// <param name="noteId"> Note Id </param>
        /// <returns> SMD(Status, Message , Data(Note Info) </returns>
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
                    return Ok(new { success = true, message = "Note has been marked Trash", data = trashNote });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Something went wrong" });
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// To change the background colour
        /// </summary>
        /// <param name="noteId"> Note Id </param>
        /// <param name="colour"> Colour name </param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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
                    return Ok(new { success = true, message = "Background Colour has been changed." , data = backgroundColour});
                }
                else
                {
                    return BadRequest(new { success = false, message = "Something went wrong" });
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Upload Image
        /// </summary>
        /// <param name="noteId"> Note Id </param>
        /// <param name="image"> Select Image </param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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
                    return Ok(new { success = true, message = "Image uploaded sucessfully", data=imageUpload });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Something went wrong" });
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Get All Note using Redis Cache
        /// </summary>
        /// <returns> All Notes of User </returns>
        [Authorize]
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
                    long userId = long.Parse(User.FindFirst("userId").Value);
                    noteList = (List<NoteEntity>)await noteBusiness.GetAll(userId);
                    serializeNoteList = JsonConvert.SerializeObject(noteList);
                    redisNoteList = Encoding.UTF8.GetBytes(serializeNoteList);
                    var options = new DistributedCacheEntryOptions()
                                 .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                                 .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    await distributedCache.SetAsync(cacheKey,redisNoteList,options);
                }
                return Ok(new { success = true, message = "All Notes", data = noteList });
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
