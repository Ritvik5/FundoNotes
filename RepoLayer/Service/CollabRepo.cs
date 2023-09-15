using CommonLayer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RepoLayer.Context;
using RepoLayer.Entities;
using RepoLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoLayer.Service
{
    /// <summary>
    /// Colab Repo Layer
    /// </summary>
    public class CollabRepo : ICollabRepo
    {
        private readonly IConfiguration configuration;
        private readonly FundoContext fundoContext;

        public CollabRepo(IConfiguration configuration, FundoContext fundoContext)
        {
            this.configuration = configuration;
            this.fundoContext = fundoContext;
        }
        /// <summary>
        /// Adding Collab to Note
        /// </summary>
        /// <param name="emailModel"> Collab email </param>
        /// <param name="noteId"> Note Id </param>
        /// <param name="userId"> User Id </param>
        /// <returns> Collab Info </returns>
        public async Task<CollabEntity> AddCollab(CollabEmailModel emailModel, long noteId, long userId)
        {
            try
            {
                CollabEntity collabEntity = new CollabEntity();
                collabEntity.UserId = userId;
                collabEntity.NoteId = noteId;
                collabEntity.CollabEmail = emailModel.Email;
                await fundoContext.Collab.AddAsync(collabEntity);
                await fundoContext.SaveChangesAsync();
                return collabEntity;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Deleting collab from Note
        /// </summary>
        /// <param name="collabId"> Collab Id</param>
        /// <param name="noteId"> Note Id </param>
        /// <param name="userId"> User Id </param>
        /// <returns> Boolean Value </returns>
        public async Task<bool> DeleteCollab(long collabId,long noteId,long userId)
        {
            try
            {
                var user = fundoContext.Collab.FirstOrDefault(u => u.NoteId == noteId && u.UserId == userId);
                if(user != null)
                {
                    fundoContext.Collab.Remove(user);
                    await fundoContext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        /// <summary>
        /// List of Collab's for Note
        /// </summary>
        /// <param name="noteId"> User Id </param>
        /// <param name="userId"> Note Id </param>
        /// <returns> List of Collab </returns>
        public async Task<List<CollabEntity>> GetAllCollabByNoteId(long noteId,long userId)
        {
            try
            {
                List<CollabEntity> user = await fundoContext.Collab.Where(u => u.NoteId == noteId && u.UserId == userId).Include(u => u.User).Include(u => u.Note).ToListAsync();
                return user;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
