using BusinessLayer.Interface;
using CommonLayer.Model;
using RepoLayer.Entities;
using RepoLayer.Interface;
using RepoLayer.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class CollabBusiness : ICollabBusiness
    {
		private readonly ICollabRepo collabRepo;
		public CollabBusiness(ICollabRepo collabRepo)
		{
			this.collabRepo = collabRepo;
		}
        public async Task<CollabEntity> AddCollab(CollabEmailModel emailModel, long noteId, long userId)
        {
			try
			{
				return await collabRepo.AddCollab(emailModel, noteId, userId);
			}
			catch (Exception)
			{

				throw;
			}
        }

        public async Task<bool> DeleteCollab(long collabId, long noteId, long userId)
        {
			try
			{
				return await collabRepo.DeleteCollab(collabId, noteId, userId);
			}
			catch (Exception)
			{

				throw;
			}
        }
		public async Task<List<CollabEntity>> GetAllCollabByNoteId(long noteId, long userId)
		{
			try
			{
				return await collabRepo.GetAllCollabByNoteId(noteId, userId);
			}
			catch (Exception)
			{

				throw;
			}
		}
    }
}
