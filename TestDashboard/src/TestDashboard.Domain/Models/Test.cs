namespace TestDashboard.Domain.Models;

public class Test
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int TimeLimit { get; set; } // in minutes
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string CreatorId { get; set; } = string.Empty;
    public virtual ApplicationUser Creator { get; set; } = null!;
    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
    public virtual ICollection<TestAttempt> Attempts { get; set; } = new List<TestAttempt>();
} 