using CommonLayer.Model;
using RepoLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepoLayer.Interface
{
    public interface ICollabRepo
    {
        Task<CollabEntity> AddCollab(CollabEmailModel emailModel, long noteId, long userId);
        Task<bool> DeleteCollab(long collabId, long noteId, long userId);
        Task<List<CollabEntity>> GetAllCollabByNoteId(long noteId, long userId);
    }
}
