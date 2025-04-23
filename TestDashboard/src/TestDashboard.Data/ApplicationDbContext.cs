using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestDashboard.Domain.Models;

namespace TestDashboard.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Test> Tests { get; set; } = null!;
    public DbSet<Question> Questions { get; set; } = null!;
    public DbSet<Answer> Answers { get; set; } = null!;
    public DbSet<TestAttempt> TestAttempts { get; set; } = null!;
    public DbSet<TestAttemptAnswer> TestAttemptAnswers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Test>()
            .HasOne(t => t.Creator)
            .WithMany(u => u.CreatedTests)
            .HasForeignKey(t => t.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Question>()
            .HasOne(q => q.Test)
            .WithMany(t => t.Questions)
            .HasForeignKey(q => q.TestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Answer>()
            .HasOne(a => a.Question)
            .WithMany(q => q.Answers)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<TestAttempt>()
            .HasOne(ta => ta.Test)
            .WithMany(t => t.Attempts)
            .HasForeignKey(ta => ta.TestId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<TestAttempt>()
            .HasOne(ta => ta.User)
            .WithMany(u => u.TestAttempts)
            .HasForeignKey(ta => ta.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<TestAttemptAnswer>()
            .HasOne(taa => taa.TestAttempt)
            .WithMany(ta => ta.Answers)
            .HasForeignKey(taa => taa.TestAttemptId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<TestAttemptAnswer>()
            .HasOne(taa => taa.Answer)
            .WithMany(a => a.AttemptAnswers)
            .HasForeignKey(taa => taa.AnswerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
} 