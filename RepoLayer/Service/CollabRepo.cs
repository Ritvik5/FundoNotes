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
    public class CollabRepo : ICollabRepo
    {
        private readonly IConfiguration configuration;
        private readonly FundoContext fundoContext;

        public CollabRepo(IConfiguration configuration, FundoContext fundoContext)
        {
            this.configuration = configuration;
            this.fundoContext = fundoContext;
        }

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
            catch (Exception)
            {

                throw;
            }
        }
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
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<CollabEntity>> GetAllCollabByNoteId(long noteId,long userId)
        {
            try
            {
                List<CollabEntity> user = await fundoContext.Collab.Where(u => u.NoteId == noteId && u.UserId == userId).Include(u => u.User).Include(u => u.Note).ToListAsync();
                return user;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
