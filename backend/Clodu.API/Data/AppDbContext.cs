using Microsoft.EntityFrameworkCore;
using Clodu.API.Models;

namespace Clodu.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    // Таблицы
    public DbSet<User> Users { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }
    public DbSet<FileData> Files { get; set; }
    public DbSet<FileKey> FileKeys { get; set; }  // 👈 ДОБАВЛЕНО
    public DbSet<Folder> Folders { get; set; }
    public DbSet<FolderFile> FolderFiles { get; set; }
    public DbSet<FolderFolder> FolderFolders { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupMember> GroupMembers { get; set; }
    public DbSet<GroupLog> GroupLogs { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<FileTag> FileTags { get; set; }
    public DbSet<Space> Spaces { get; set; }
    public DbSet<SpaceTag> SpaceTags { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<UserSubscriptionHistory> UserSubscriptionHistory { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ==================== Users ====================
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.Username).IsUnique();
            entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
        });
        
        // ==================== User Sessions ====================
        modelBuilder.Entity<UserSession>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.JwtToken);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsRevoked).HasDefaultValue(false);
            
            entity.HasOne(e => e.User)
                .WithMany(u => u.Sessions)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // ==================== FileData ====================
        modelBuilder.Entity<FileData>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.Hash);
            entity.HasIndex(e => e.DeletedAt);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Type).HasMaxLength(100);
            entity.Property(e => e.Hash).HasMaxLength(64);
            entity.Property(e => e.SizeBytes).HasDefaultValue(0);
            entity.Property(e => e.TotalShards).HasDefaultValue(16);
            entity.Property(e => e.DataShards).HasDefaultValue(10);
            entity.Property(e => e.ShardLocations).HasColumnType("jsonb").HasDefaultValue("[]");
            entity.Property(e => e.AddedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            entity.HasOne(e => e.User)
                .WithMany(u => u.Files)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // ==================== FileKeys ====================
        modelBuilder.Entity<FileKey>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.FileId).IsUnique();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.EncryptedKey).HasColumnType("bytea");
            entity.Property(e => e.IV).HasColumnType("bytea");
        });
        
        // ==================== Folders ====================
        modelBuilder.Entity<Folder>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.OwnerId, e.OwnerType });
            entity.HasIndex(e => e.DeletedAt);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.OwnerType).IsRequired().HasMaxLength(10);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });
        
        // ==================== Folder Files ====================
        modelBuilder.Entity<FolderFile>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.FolderId);
            entity.HasIndex(e => e.FileId);
            entity.HasIndex(e => new { e.FolderId, e.FileId }).IsUnique();
            
            entity.HasOne(e => e.Folder)
                .WithMany(f => f.FolderFiles)
                .HasForeignKey(e => e.FolderId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.File)
                .WithMany(f => f.FolderFiles)
                .HasForeignKey(e => e.FileId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.Space)
                .WithMany(s => s.FolderFiles)
                .HasForeignKey(e => e.SpaceId)
                .OnDelete(DeleteBehavior.SetNull);
        });
        
        // ==================== Folder Folders ====================
        modelBuilder.Entity<FolderFolder>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ParentId);
            entity.HasIndex(e => e.ChildId);
            entity.HasIndex(e => new { e.ParentId, e.ChildId }).IsUnique();
            
            entity.HasOne(e => e.Parent)
                .WithMany(f => f.ChildFolders)
                .HasForeignKey(e => e.ParentId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.Child)
                .WithMany(f => f.ParentFolders)
                .HasForeignKey(e => e.ChildId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // ==================== Groups ====================
        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.OwnerId);
            entity.HasIndex(e => e.DeletedAt);
            entity.Property(e => e.Groupname).IsRequired().HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            entity.HasOne(e => e.Owner)
                .WithMany(u => u.OwnedGroups)
                .HasForeignKey(e => e.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // ==================== Group Members ====================
        modelBuilder.Entity<GroupMember>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.GroupId);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => new { e.GroupId, e.UserId }).IsUnique();
            entity.Property(e => e.Rights).HasDefaultValue(3);
            entity.Property(e => e.AddedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            entity.HasOne(e => e.Group)
                .WithMany(g => g.Members)
                .HasForeignKey(e => e.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.User)
                .WithMany(u => u.GroupMembers)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // ==================== Group Logs ====================
        modelBuilder.Entity<GroupLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.GroupId);
            entity.HasIndex(e => e.UserId);
            entity.Property(e => e.EditedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            entity.HasOne(e => e.Group)
                .WithMany(g => g.Logs)
                .HasForeignKey(e => e.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        });
        
        // ==================== Tags ====================
        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
        });
        
        // ==================== File Tags ====================
        modelBuilder.Entity<FileTag>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.TagId);
            entity.HasIndex(e => e.FileId);
            entity.HasIndex(e => new { e.TagId, e.FileId }).IsUnique();
            
            entity.HasOne(e => e.Tag)
                .WithMany(t => t.FileTags)
                .HasForeignKey(e => e.TagId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.File)
                .WithMany(f => f.FileTags)
                .HasForeignKey(e => e.FileId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // ==================== Spaces ====================
        modelBuilder.Entity<Space>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.OwnerId);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            
            entity.HasOne(e => e.Owner)
                .WithMany(u => u.Spaces)
                .HasForeignKey(e => e.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // ==================== Space Tags ====================
        modelBuilder.Entity<SpaceTag>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.SpaceId);
            entity.HasIndex(e => e.TagId);
            entity.HasIndex(e => new { e.SpaceId, e.TagId }).IsUnique();
            
            entity.HasOne(e => e.Space)
                .WithMany(s => s.SpaceTags)
                .HasForeignKey(e => e.SpaceId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.Tag)
                .WithMany(t => t.SpaceTags)
                .HasForeignKey(e => e.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // ==================== Subscriptions ====================
        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Price).HasDefaultValue(0);
            entity.Property(e => e.MaxStorageBytes).HasDefaultValue(5368709120L);
            entity.Property(e => e.MaxFileSizeMB).HasDefaultValue(100);
            entity.Property(e => e.MaxTeamMembers).HasDefaultValue(1);
            entity.Property(e => e.Features).HasColumnType("jsonb").HasDefaultValue("{}");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });
        
        // ==================== User Subscription History ====================
        modelBuilder.Entity<UserSubscriptionHistory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.SubscriptionId);
            entity.HasIndex(e => new { e.UserId, e.IsActive });
            entity.HasIndex(e => e.EndsAt);
            entity.Property(e => e.StartedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            
            entity.HasOne(e => e.User)
                .WithMany(u => u.SubscriptionHistory)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.Subscription)
                .WithMany(s => s.UserHistory)
                .HasForeignKey(e => e.SubscriptionId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}