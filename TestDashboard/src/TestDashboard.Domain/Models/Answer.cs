namespace TestDashboard.Domain.Models;

public class Answer
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public int QuestionId { get; set; }
    public virtual Question Question { get; set; } = null!;
    public virtual ICollection<TestAttemptAnswer> AttemptAnswers { get; set; } = new List<TestAttemptAnswer>();
} 