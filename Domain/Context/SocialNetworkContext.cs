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
        public DbSet<Post> Posts { get; set; }

        public SocialNetworkContext(DbContextOptions<SocialNetworkContext> options)
        : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(x =>
            {
                x.Property(y => y.Id).HasDefaultValueSql("NEWID()");
            });

            modelBuilder.Entity<Like>(x =>
            {
                x.Property(y => y.Id).HasDefaultValueSql("NEWID()");
                x.HasOne(y => y.User)
                .WithMany(z => z.Likes)
                .HasForeignKey(y => y.UserId);

                x.HasOne(y => y.Post)
                .WithMany(z => z.Likes)
                .HasForeignKey(y => y.PostId);
            });

            modelBuilder.Entity<Post>(x =>
            {
                x.Property(y => y.Id).HasDefaultValueSql("NEWID()");
                x.HasOne(y => y.User)
                .WithMany(z => z.Posts)
                .HasForeignKey(y => y.UserId);
            });
        }
    }
}
