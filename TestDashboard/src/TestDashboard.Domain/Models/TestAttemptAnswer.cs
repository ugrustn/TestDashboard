namespace TestDashboard.Domain.Models;

public class TestAttemptAnswer
{
    public int Id { get; set; }
    public int TestAttemptId { get; set; }
    public int AnswerId { get; set; }
    public DateTime AnsweredAt { get; set; }
    public virtual TestAttempt TestAttempt { get; set; } = null!;
    public virtual Answer Answer { get; set; } = null!;
} 