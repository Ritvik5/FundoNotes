using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepoLayer.Context;
using RepoLayer.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundoNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollabController : ControllerBase
    {
        private readonly ICollabBusiness collabBusiness;
        private readonly FundoContext fundoContext;

        public CollabController(ICollabBusiness collabBusiness,FundoContext fundoContext)
        {
            this.collabBusiness = collabBusiness;
            this.fundoContext = fundoContext;
        }
        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddCollab(string collabEmail,long noteId)
        {
            try
            {
                var userIdClaim = User.FindFirst("userId");
                long userId = long.Parse(userIdClaim.Value);
                var addCollab = await collabBusiness.AddCollab(collabEmail,noteId,userId);
                if(addCollab != null)
                {
                    return Ok(new { success = true, message = "New Collaborator added.",data = addCollab });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Something went wrong", data = addCollab });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteCollab(long collabId,long noteId)
        {
            try
            {
                var userIdClaim = User.FindFirst("userId");
                long userId = long.Parse(userIdClaim.Value);
                bool deleteCollab = await collabBusiness.DeleteCollab(collabId, noteId, userId);
                if(deleteCollab == true)
                {
                    return Ok(new { success = true, message = "Collaborator removed." });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Failed to remove Collaborator." });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllColaborator(long noteId)
        {
            try
            {
                var userIdClaim = User.FindFirst("userId");
                long userId = long.Parse(userIdClaim.Value);
                List<CollabEntity> result = new List<CollabEntity>();
                result = await collabBusiness.GetAllCollabByNoteId(noteId, userId);
                if(result != null)
                {
                    return Ok(new { success = true, message = "All Collaborator for given note.",data= result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Failed to fetch all Collaborator.", data = result });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}
