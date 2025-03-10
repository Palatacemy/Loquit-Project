﻿using Loquit.Data.Entities;
using Loquit.Data.Entities.Abstractions;
using Loquit.Data.Entities.ChatTypes;
using Loquit.Data.Entities.MessageTypes;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Reflection.Emit;

namespace Loquit.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<DirectChat> DirectChats { get; set; }
        public DbSet<GroupChat> GroupChats { get; set; }
        public DbSet<ChatUser> ChatUsers { get; set; }
        public DbSet<ImageMessage> ImageMessages { get; set; }
        public DbSet<TextMessage> TextMessages { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Dislike> Dislikes { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Save> Saves { get; set; }
        public DbSet<Comment> Comments { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                .HasMany(u => u.Posts)
                .WithOne(p => p.Creator);

            builder.Entity<Post>()
                .HasOne(p => p.Creator)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.CreatorId);

            builder.Entity<Comment>()
                .HasOne(c => c.Commenter)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.CommenterId)
                .OnDelete(DeleteBehavior.Restrict);

            /*builder.Entity<Comment>()
                .HasOne(m => m.Parent)
                .WithMany(m => m.Replies)
                .HasForeignKey(m => m.ParentId)
                .OnDelete(DeleteBehavior.Restrict);*/

            builder.Entity<ChatUser>()
                .HasKey(cu => new { cu.ChatId, cu.UserId });

            builder.Entity<ChatUser>()
                .HasOne(cu => cu.Chat)
                .WithMany(c => c.Members)
                .HasForeignKey(cu => cu.ChatId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ChatUser>()
                .HasOne(cu => cu.User)
                .WithMany(u => u.Chats)
                .HasForeignKey(cu => cu.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FriendRequest>()
                .HasOne(l => l.SentByUser)
                .WithMany(p => p.FriendRequestsSent)
                .HasForeignKey(l => l.SentByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FriendRequest>()
                .HasOne(l => l.SentToUser)
                .WithMany(u => u.FriendRequestsReceived)
                .HasForeignKey(l => l.SentToUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Like>()
                .HasOne(l => l.Post)
                .WithMany(p => p.LikedBy)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.LikedPosts)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Dislike>()
                .HasOne(l => l.Post)
                .WithMany(p => p.DislikedBy)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Dislike>()
                .HasOne(l => l.User)
                .WithMany(u => u.DislikedPosts)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Save>()
                .HasOne(l => l.Post)
                .WithMany(p => p.SavedBy)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Save>()
                .HasOne(l => l.User)
                .WithMany(u => u.SavedPosts)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);
            return new ApplicationDbContext(builder.Options);
        }
    }
}
