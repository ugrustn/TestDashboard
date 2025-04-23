namespace TestDashboard.Domain.Models;

public class TestAttempt
{
    public int Id { get; set; }
    public int TestId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int? Score { get; set; }
    public virtual Test Test { get; set; } = null!;
    public virtual ApplicationUser User { get; set; } = null!;
    public virtual ICollection<TestAttemptAnswer> Answers { get; set; } = new List<TestAttemptAnswer>();
} 