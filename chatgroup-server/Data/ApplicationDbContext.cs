using chatgroup_server.Models;
using Microsoft.EntityFrameworkCore;

namespace chatgroup_server.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions options):base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Friends> Friends { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMessages> GroupMessages {  get; set; }    
        public DbSet<GroupDetail> GroupDetails {  get; set; }   
        public DbSet<UserMessages> UserMessages { get; set; }
        public DbSet<Files> Files { get; set; }
        public DbSet<GroupMessageFile> GroupMessageFiles { get; set; }
        public DbSet<UserMessageFile> UserMessageFiles { get; set; }
        public DbSet<UserMessageReaction> UserMessageReactions { get; set; }
        public DbSet<GroupMessageReaction> GroupMessageReactions { get; set; }
        public DbSet<GroupMessageStatus> GroupMessageStatuses { get; set; }
        public DbSet<UserMessageStatus> UserMessageStatuses { get; set; }
        public DbSet<UserRefreshToken> UserRefreshTokens {  get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<CloudMessage> CloudMessages { get; set; }
        public DbSet<CloudMessageFile> CloudMessageFiles { get; set; }  
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Friends>().HasOne(f => f.User).WithMany(u => u.Friends).HasForeignKey(f=>f.UserId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Friends>().HasOne(f=>f.Friend).WithMany().HasForeignKey(f=>f.FriendId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<UserMessages>().HasOne(u => u.Sender).WithMany(u => u.Senders).HasForeignKey(x => x.SenderId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserMessages>().HasOne(u=>u.Receiver).WithMany(u=>u.Receivers).HasForeignKey(x=>x.ReceiverId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<GroupMessageReaction>()
        .HasOne(gmr => gmr.User)
        .WithMany(u => u.groupMessageReactions)
        .HasForeignKey(gmr => gmr.UserId)
        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserMessageReaction>()
                .HasOne(umr => umr.User)
                .WithMany(u => u.userMessageReactions)
                .HasForeignKey(umr => umr.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserMessageStatus>()
                .HasOne(ums => ums.Receiver)
                .WithMany(u => u.userMessageStatuses)
                .HasForeignKey(ums => ums.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<GroupMessageStatus>()
                .HasOne(gms => gms.Receiver)
                .WithMany(u => u.groupMessageStatuses)
                .HasForeignKey(gms => gms.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);
            base.OnModelCreating(modelBuilder);
        }
    } 
}
