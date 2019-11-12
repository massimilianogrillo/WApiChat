using Lavoro.Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace Lavoro.Data
{
    public class LaboroContext : DbContext
    {
        public LaboroContext(DbContextOptions<LaboroContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Messages> Messages { get; set; }
        public DbSet<File> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Contact>()
                .HasKey(s => new { s.UserId, s.ContactId });
            modelBuilder.Entity<Contact>()
                .HasOne(x => x.User)
                .WithMany(m => m.MyContacts)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Contact>()
                .HasOne(x => x.ContactUser)
                .WithMany(m => m.TheirContacts)
                .HasForeignKey(x => x.ContactId)
                .OnDelete(DeleteBehavior.Cascade);

            

            modelBuilder.Entity<UserGroup>()
                .HasKey(s => new { s.UserId, s.GroupId });
            modelBuilder.Entity<UserGroup>()
                .HasOne(x => x.Group)
                .WithMany(m => m.Users)
                .HasForeignKey(x => x.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserGroup>()
                .HasOne(x => x.User)
                .WithMany(m => m.UserGroups)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Chat>()
                .HasOne(x => x.StarterUser)
                .WithMany(m => m.OutChats)
                .HasForeignKey(x => x.StarterUserId);
            modelBuilder.Entity<Chat>()
                .HasOne(x => x.OtherUser)
                .WithMany(m => m.InChats)
                .HasForeignKey(x => x.OtherUserId);

            modelBuilder.Entity<Chat>()
            .HasMany(t => t.Messages)
            .WithOne(t => t.Chat)
            .HasForeignKey(t => t.ChatId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Chat>()
                .HasOne(x => x.LastMessageSeenByStarterUser)
                .WithOne()
                .HasForeignKey(typeof(Chat), nameof(Chat.IdLastMessageSeenByStarterUser));
            modelBuilder.Entity<Chat>()
                .HasOne(x => x.LastMessageSeenByOtherUser)
                .WithOne()
                .HasForeignKey(typeof(Chat), nameof(Chat.IdLastMessageSeenByOtherUser));


            //This properties will not be in the DB
            modelBuilder.Entity<File>().Ignore(e => e.Type);
            modelBuilder.Entity<User>().Ignore(e => e.Status);
            modelBuilder.Entity<User>().Ignore(e => e.Chats);
            modelBuilder.Entity<Chat>().Ignore(c => c.UnreadStarterUser);
            modelBuilder.Entity<Chat>().Ignore(c => c.UnreadOtherUser);

        }

#pragma warning disable
        /// <summary>
        /// This method is used to insert new data with identity_insert on a table on
        /// </summary>
        /// <param name="table"></param>
        [CLSCompliant(false)]
        public void SaveChangesWithIdentityInsert(string table = "Users")
        {
            Database.OpenConnection();

            Database.ExecuteSqlCommand("SET IDENTITY_INSERT " + table + " ON");
            SaveChanges();
            Database.ExecuteSqlCommand("SET IDENTITY_INSERT " + table + " OFF");
            Database.CloseConnection();
        }


    }
}
