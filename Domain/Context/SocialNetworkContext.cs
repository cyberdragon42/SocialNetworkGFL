using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Domain.Context
{
    public class SocialNetworkContext : IdentityDbContext<User>
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public SocialNetworkContext(DbContextOptions<SocialNetworkContext> options)
        : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
                x.HasKey(nameof(Subscription.FollowerId), nameof(Subscription.FollowingId));
                x.HasOne(y => y.Follower)
                    .WithMany(z => z.Followings)
                    .HasForeignKey(y => y.FollowerId)
                    .OnDelete(DeleteBehavior.ClientCascade);

                x.HasOne(y => y.Following)
                    .WithMany(z => z.Followers)
                    .HasForeignKey(y => y.FollowingId)
                    .OnDelete(DeleteBehavior.ClientCascade);
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

            modelBuilder.Entity<Notification>(x =>
            {
                x.Property(y => y.Id).HasDefaultValueSql("NEWID()");
                x.HasOne(y => y.Reciever)
                .WithMany(z => z.Notifications)
                .HasForeignKey(y => y.RecieverId);
            });

            //modelBuilder.Entity<Avatar>(x =>
            //{
            //    x.Property(y => y.Id).HasDefaultValueSql("NEWID()");
            //});
        }
    }
}
