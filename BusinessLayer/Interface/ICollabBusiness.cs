using RepoLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface ICollabBusiness
    {
        Task<CollabEntity> AddCollab(string collabEmail, long noteId, long userId);
        Task<bool> DeleteCollab(long collabId, long noteId, long userId);
        Task<List<CollabEntity>> GetAllCollabByNoteId(long noteId, long userId);
    }
}
