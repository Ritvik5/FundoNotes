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
    /// Label Repo Layer
    /// </summary>
    public class LabelRepo : ILabelRepo
    { 
        private readonly IConfiguration configuration;
        private readonly FundoContext fundoContext;
        public LabelRepo(IConfiguration configuration, FundoContext fundoContext)
        {
            this.configuration = configuration;
            this.fundoContext = fundoContext;
        }
        /// <summary>
        /// Add Label to Note
        /// </summary>
        /// <param name="labelName"> label name </param>
        /// <param name="noteId"> Note Id </param>
        /// <param name="userId"> User Id </param>
        /// <returns> Label Info </returns>
        public async Task<LabelEntity> AddLabel(string labelName,long noteId,long userId)
        {
            try
            {
                var user = fundoContext.Users.FirstOrDefault(u => u.UserId == userId);
                var note = fundoContext.Notes.FirstOrDefault(n => n.NoteId == noteId);
                LabelEntity label = new LabelEntity
                {
                    User = user,
                    Note = note
                };
                label.LabelName = labelName;
                fundoContext.Label.Add(label);
                await fundoContext.SaveChangesAsync();
                return label;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Update Label 
        /// </summary>
        /// <param name="labelName"> Label Name </param>
        /// <param name="userId"> User Id </param>
        /// <param name="labelId"> Label Id </param>
        /// <returns> Label Info </returns>
        public async Task<LabelEntity> UpdateLabel(string labelName,long userId,long labelId)
        {
            try
            {
                var result = fundoContext.Label.FirstOrDefault(u => u.UserId == userId && u.LabelId == labelId);
                if(result != null)
                {
                    result.LabelName = labelName;
                    await fundoContext.SaveChangesAsync();
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Delete Label 
        /// </summary>
        /// <param name="labelId"> Label Id </param>
        /// <param name="userId"> User Id </param>
        /// <returns> Boolean Value </returns>
        /// <exception cref="Exception"></exception>
        public async Task DeleteLabel(long labelId,long userId)
        {
            try
            {
                var label = await fundoContext.Label.FirstOrDefaultAsync(u=> u.UserId == userId && u.LabelId == labelId);
                if(label != null)
                {
                    fundoContext.Label.Remove(label);
                    await fundoContext.SaveChangesAsync();
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        /// <summary>
        /// Get all Label of User
        /// </summary>
        /// <param name="userId"> User Id </param>
        /// <returns> Label Info </returns>
        public async Task<List<LabelEntity>> GetLabel(long userId)
        {
            try
            {
                List<LabelEntity> label = await fundoContext.Label.Where(u => u.UserId == userId).Include(u => u.User).Include(n => n.Note).ToListAsync();
                return label;
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        /// <summary>
        /// Get all Label for a Note
        /// </summary>
        /// <param name="noteId"> Note Id </param>
        /// <returns> Label Info </returns>
        public async Task<List<LabelEntity>> GetLabelByNoteId(long noteId)
        {
            try
            {
                List<LabelEntity> labelByNoteId = await fundoContext.Label.Where(u => u.NoteId == noteId).Include(u => u.User).Include(n=>n.Note).ToListAsync();
                return labelByNoteId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
