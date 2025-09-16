using FF7_remake.Models;
using Microsoft.EntityFrameworkCore;

namespace FF7_remake.DBContext;

public class FF7DbContext : DbContext
{
    public FF7DbContext(DbContextOptions<FF7DbContext> options) : base(options)
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
            entity.Property(e => e.SavedState).IsRequired();
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
        
        SeedData(modelBuilder);
    }
    
    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Chapters
        modelBuilder.Entity<Chapter>().HasData(
            new Chapter { ChapterId = 1, Title = "The Destruction of Mako Reactor 1", Summary = "Cloud joins AVALANCHE for their first mission", Quiz = "Basic combat and story questions" },
            new Chapter { ChapterId = 2, Title = "Fateful Encounters", Summary = "Cloud meets Aerith in the slums", Quiz = "Character interaction quiz" },
            new Chapter { ChapterId = 3, Title = "Home Sweet Slum", Summary = "Exploring Sector 7 slums", Quiz = "Exploration and side quest quiz" },
            new Chapter { ChapterId = 4, Title = "Mad Dash", Summary = "Escaping Shinra forces", Quiz = "Action sequence quiz" },
            new Chapter { ChapterId = 5, Title = "Dogged Pursuit", Summary = "The reactor 5 mission begins", Quiz = "Strategy and combat quiz" }
        );

        // Seed sample user
        modelBuilder.Entity<User>().HasData(
            new User 
            { 
                UserId = 1, 
                Username = "CloudStrife", 
                Email = "cloud@avalanche.com", 
                Password = BCrypt.Net.BCrypt.HashPassword("buster_sword"), 
                Progress = "Chapter 1 completed",
                CreatedAt = DateTime.UtcNow,
            }
        );

        modelBuilder.Entity<Quiz>().HasData(
            new Quiz 
            { 
                QuizId = 1, 
                ChapterId = 1, 
                Question = "What is Cloud's weapon?|Who leads AVALANCHE?|What is the target of the mission?", 
                CorrectAnswer = "Buster Sword|Barret|Mako Reactor 1", 
                Badges = "First Strike,Reactor Destroyer" 
            }
        );
    }
}