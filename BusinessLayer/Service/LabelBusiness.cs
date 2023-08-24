using BusinessLayer.Interface;
using RepoLayer.Entities;
using RepoLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class LabelBusiness : ILabelBusiness
    {
        private readonly ILabelRepo labelRepo;
        public LabelBusiness(ILabelRepo labelRepo)
        {
            this.labelRepo = labelRepo;
        }
        public async Task<LabelEntity> AddLabel(string labelName, long noteId, long userId)
        {
            try
            {
                return await labelRepo.AddLabel(labelName, noteId, userId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task DeleteLabel(long labelId, long userId)
        {
            try
            {
                await labelRepo.DeleteLabel(labelId, userId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<LabelEntity> UpdateLabel(string labelName, long userId, long labelId)
        {
            try
            {
                return await labelRepo.UpdateLabel(labelName, userId, labelId);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<LabelEntity>> GetLabel(long userId)
        {
            try
            {
                return await labelRepo.GetLabel(userId);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<LabelEntity>> GetLabelByNoteId(long noteId)
        {
            try
            {
                return await labelRepo.GetLabelByNoteId(noteId);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
