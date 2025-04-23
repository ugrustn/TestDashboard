namespace TestDashboard.Domain.Models;

public class Question
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public int Points { get; set; }
    public int TestId { get; set; }
    public virtual Test Test { get; set; } = null!;
    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
} 