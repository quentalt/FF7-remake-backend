using FF7_remake.Models;
using Microsoft.EntityFrameworkCore;

namespace FF7_remake.DBContext;

public class Ff7DbContext : DbContext
{
    public Ff7DbContext(DbContextOptions<Ff7DbContext> options) : base(options)
    {}
    
    public DbSet<Chapter> Chapters { get; set; }
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserProgress> UserProgresses { get; set; }
    public DbSet<Leaderboard> Leaderboards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Chapter>(entity =>
        {
            entity.ToTable("Chapters");
            entity.HasKey(e => e.ChapterId);
            entity.Property(e => e.Title).IsRequired();
        });

        modelBuilder.Entity<Quiz>(entity =>
        {
            entity.ToTable("Quizzes");
            entity.HasKey(e => e.QuizId);
            entity.HasOne(q => q.Chapter)
                .WithMany(c => c.Quizzes)
                .HasForeignKey(q => q.ChapterId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.UserId);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.Username).IsUnique();
            entity.Property(e => e.Username).IsRequired();
            entity.Property(e => e.Email).IsRequired();
            entity.Property(e => e.Password).IsRequired();
        });
        
        modelBuilder.Entity<UserProgress>(entity =>
        {
            entity.ToTable("UserProgresses");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.ChapterId }).IsUnique();
          
            entity.HasOne(up => up.User)
                .WithMany(u => u.UserProgresses)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(up => up.Chapter)
                .WithMany(c => c.UserProgresses)
                .HasForeignKey(up => up.ChapterId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.Property(e => e.LastUpdated).IsRequired();
        });
        
        modelBuilder.Entity<Leaderboard>(entity =>
        {
            entity.ToTable("Leaderboards");
            entity.HasKey(e => e.LeaderBoardId);
            entity.HasOne(l => l.User)
                .WithMany(u => u.LeaderboardEntries)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.Property(e => e.Score).IsRequired();
        });
    }

   
}