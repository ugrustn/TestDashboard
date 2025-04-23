using Microsoft.AspNetCore.Identity;

namespace TestDashboard.Domain.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public virtual ICollection<Test> CreatedTests { get; set; } = new List<Test>();
    public virtual ICollection<TestAttempt> TestAttempts { get; set; } = new List<TestAttempt>();
} 