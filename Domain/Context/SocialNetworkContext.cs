using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Context
{
    public class SocialNetworkContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public SocialNetworkContext(DbContextOptions<SocialNetworkContext> options)
        : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(x =>
            {
                x.Property(y => y.Id).HasDefaultValueSql("NEWID()");
            });
        }
    }
}
