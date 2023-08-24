using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace RepoLayer.Entities
{
    public class CollabEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CollabId { get; set; }
        public string CollabEmail { get; set; }
        [ForeignKey("Users")]
        public long NoteId { get; set; }
        [JsonIgnore]
        public virtual NoteEntity Note { get; set; }
        [ForeignKey("Notes")]
        public long UserId { get; set; }
        [JsonIgnore]
        public virtual UserEntity User { get; set;}
    }
}
