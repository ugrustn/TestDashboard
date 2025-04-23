namespace TestDashboard.API.DTOs;

public record LoginRequest(string Email, string Password);

public record RegisterRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName
);

public record AuthResponse(
    bool Succeeded,
    string Token,
    string? Error = null
); 