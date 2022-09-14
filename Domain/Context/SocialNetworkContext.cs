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
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public SocialNetworkContext(DbContextOptions<SocialNetworkContext> options)
        : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(x =>
            {
                x.Property(y => y.Id).HasDefaultValueSql("NEWID()");
            });

            modelBuilder.Entity<Comment>(x =>
            {
                x.Property(y => y.Id).HasDefaultValueSql("NEWID()");
                x.HasOne(y => y.Post)
                    .WithMany(z => z.Comments)
                    .HasForeignKey(y => y.PostId);
                x.HasOne(y => y.User)
                    .WithMany(z => z.Comments)
                    .HasForeignKey(y => y.UserId);
            });

            modelBuilder.Entity<Subscription>(x =>
            {
                x.Property(y => y.Id).HasDefaultValueSql("NEWID()");
                x.HasOne(y => y.Follower)
                    .WithMany(z => z.Followings)
                    .HasForeignKey(y => y.FollowerId);

                x.HasOne(y => y.Following)
                    .WithMany(z => z.Followers)
                    .HasForeignKey(y => y.FollowingId);
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
