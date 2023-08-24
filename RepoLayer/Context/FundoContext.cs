using Microsoft.EntityFrameworkCore;
using RepoLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepoLayer.Context
{
    public class FundoContext : DbContext
    {
        public FundoContext(DbContextOptions options) : base (options)
        {
        }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<NoteEntity> Notes { get; set; }
        public DbSet<CollabEntity> Collab { get; set; }
        public DbSet<LabelEntity> Label { get; set; }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<NoteEntity>()
        //        .HasOne(n => n.User)
        //        .WithMany(u => u.Notes)
        //        .HasForeignKey(u => u.UserId)
        //        .OnDelete(DeleteBehavior.Cascade);

        //    base.OnModelCreating(modelBuilder);
        //}
    }
}
