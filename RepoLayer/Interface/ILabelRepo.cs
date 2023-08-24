using RepoLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepoLayer.Interface
{
    public interface ILabelRepo
    {
        Task<LabelEntity> AddLabel(string labelName, long noteId, long userId);
        Task<LabelEntity> UpdateLabel(string labelName, long userId, long labelId);
        Task DeleteLabel(long labelId, long userId);
        Task<List<LabelEntity>> GetLabel(long userId);
        Task<List<LabelEntity>> GetLabelByNoteId(long noteId);
    }
}
