using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepoLayer.Context;
using RepoLayer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FundoNotes.Controllers
{
    /// <summary>
    /// Label Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabelBusiness labelBusiness;
        private readonly FundoContext fundoContext;
        public LabelController(ILabelBusiness labelBusiness, FundoContext fundoContext)
        {
            this.labelBusiness = labelBusiness;
            this.fundoContext = fundoContext;
        }
        /// <summary>
        /// Add label 
        /// </summary>
        /// <param name="labelName"> Label Name </param>
        /// <param name="noteId"> Note Id </param>
        /// <returns> SMD(Success, Message, Data(Label Info)) </returns>
        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddLabel(string labelName,long noteId)
        {
            try
            {
                var userIdClaim = User.FindFirst("userId");
                long userId = long.Parse(userIdClaim.Value);
                var addLabel = await labelBusiness.AddLabel(labelName, noteId,userId);
                if(addLabel != null)
                {
                    return Ok(new {success = true,message ="Label Added",data= addLabel});
                }
                else
                {
                    return BadRequest(new { success = false, message = "Failed to add label", data = addLabel });
                }
            }
            catch (Exception ex )
            {
                throw new Exception(ex.Message) ;
            }
        }
        /// <summary>
        /// Update Label 
        /// </summary>
        /// <param name="labelName"> Label Name </param>
        /// <param name="labelId"> Label Id </param>
        /// <returns> SMD(Success, Message, Data(Updated Label Info)) </returns>
        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateLabel(string labelName, long labelId)
        {
            try
            {
                var userIdClaim = User.FindFirst("userId");
                long userId = long.Parse(userIdClaim.Value);
                var updateLabel = await labelBusiness.UpdateLabel(labelName,userId,labelId);
                if(updateLabel != null)
                {
                    return Ok(new { success = true, message = "Label Updated", data = updateLabel });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Failed to update label", data = updateLabel });
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        /// <summary>
        /// Delete Label
        /// </summary>
        /// <param name="labelId"> Label Id</param>
        /// <returns> SMD(Success, Message, Data(Boolean Value))</returns>
        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteLabel(long labelId)
        {
            try
            {
                var userIdClaim = User.FindFirst("userId");
                long userId = long.Parse(userIdClaim.Value);
                if(userId != 0 && labelId != 0)
                {
                    await labelBusiness.DeleteLabel(labelId, userId);
                    return Ok(new { success = true, message = "Label Deleted Successfully" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Failed to delete label" });
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        /// <summary>
        /// All Labels by User.
        /// </summary>
        /// <returns> List of All Label for User</returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetLabel()
        {
            try
            {
                var userIdClaim = User.FindFirst("userId");
                long userId = long.Parse(userIdClaim.Value);
                List<LabelEntity> labels = new List<LabelEntity>();
                labels = await labelBusiness.GetLabel(userId);
                if(labels!= null)
                {
                    return Ok(new { success = true, message = "All Label by "+userId ,data = labels});
                }
                else { return BadRequest(new { success = false, message = "Failed to fetch label" }); }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// All Labels by Note.
        /// </summary>
        /// <param name="noteId"> Note Id </param>
        /// <returns> List of All Label for a Note</returns>
        [Authorize]
        [HttpGet("noteId")]
        public async Task<IActionResult> GetLabelByNoteId(long noteId)
        {
            try
            {
                List<LabelEntity> labelByNoteId = new List<LabelEntity>();
                labelByNoteId = await labelBusiness.GetLabelByNoteId(noteId);
                if (labelByNoteId != null)
                {
                    return Ok(new { success = true, message = "All Label by " + noteId, data = labelByNoteId });
                }
                else { return BadRequest(new { success = false, message = "Failed to fetch label" }); }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
