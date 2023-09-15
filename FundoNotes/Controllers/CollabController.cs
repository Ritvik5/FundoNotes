using BusinessLayer.Interface;
using CommonLayer.Model;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepoLayer.Context;
using RepoLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FundoNotes.Controllers
{
    /// <summary>
    /// Collab Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CollabController : ControllerBase
    {
        private readonly ICollabBusiness collabBusiness;
        private readonly IBus bus;

        public CollabController(ICollabBusiness collabBusiness,IBus bus)
        {
            this.collabBusiness = collabBusiness;
            this.bus = bus;
        }
        /// <summary>
        /// Add Collab
        /// </summary>
        /// <param name="emailModel"> Email for collab </param>
        /// <param name="noteId"> Note Id </param>
        /// <returns> SMD(Success,Message,Data(Collab Info)) </returns>
        [HttpPost("add")]
        public async Task<IActionResult> AddCollab(CollabEmailModel emailModel,long noteId)
        {
            try
            {
                var userIdClaim = User.FindFirst("userId");
                long userId = long.Parse(userIdClaim.Value);
                var addCollab = await collabBusiness.AddCollab(emailModel, noteId,userId);
                if(addCollab != null)
                {
                    return Ok(new { success = true, message = "New Collaborator added.",data = addCollab });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Something went wrong", data = addCollab });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Deleting Collab
        /// </summary>
        /// <param name="collabId"> Collab Id </param>
        /// <param name="noteId"> User Id </param>
        /// <returns>SMD(Success,Message) </returns>
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// All Collabs for Note
        /// </summary>
        /// <param name="noteId"> Note Id </param>
        /// <returns> SMD(Success ,Message ,Data(List of collab)) </returns>
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
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        /// <summary>
        /// Sending Mail for Collab
        /// </summary>
        /// <param name="emailModel"> Email </param>
        /// <param name="noteId"> Note Id </param>
        /// <returns> SMD(Success ,Message ,Data(Collab Info))</returns>
        [HttpPost("send")]
        public async Task<IActionResult> SendCollabMail(CollabEmailModel emailModel,long noteId)
        {
            try
            {
                var userIdClaim = User.FindFirst("userId");
                long userId = long.Parse(userIdClaim.Value);
                var result = await collabBusiness.AddCollab(emailModel,noteId,userId);

                if(result != null)
                {
                    // RabbitMQ Publisher

                    Uri uri = new Uri("rabbitmq://localhost/mailToConsumer");
                    var endPoint = await bus.GetSendEndpoint(uri);
                    await endPoint.Send(result);

                    // calling SendingMail Function

                    SendColabMail colabMail = new SendColabMail();
                    colabMail.EmailService(emailModel.Email);
                    return Ok(new { success = true,message ="Mail send to consumer.", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Mail can't be send" });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
