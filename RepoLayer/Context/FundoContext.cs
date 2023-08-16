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
    }
}
