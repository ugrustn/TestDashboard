namespace TestDashboard.API.DTOs;

public record CreateTestRequest(
    string Title,
    string Description,
    int TimeLimit,
    IEnumerable<CreateQuestionDto> Questions
);

public record CreateQuestionDto(
    string Text,
    int Points,
    IEnumerable<CreateAnswerDto> Answers
);

public record CreateAnswerDto(
    string Text,
    bool IsCorrect
);

public record TestDto(
    int Id,
    string Title,
    string Description,
    int TimeLimit,
    DateTime CreatedAt,
    string CreatorId,
    string CreatorName
);

public record TestDetailsDto(
    int Id,
    string Title,
    string Description,
    int TimeLimit,
    DateTime CreatedAt,
    string CreatorId,
    string CreatorName,
    IEnumerable<QuestionDto> Questions
);

public record QuestionDto(
    int Id,
    string Text,
    int Points,
    IEnumerable<AnswerDto> Answers
);

public record AnswerDto(
    int Id,
    string Text,
    bool IsCorrect
);

public record TestAttemptDto(
    int Id,
    int TestId,
    string UserId,
    DateTime StartTime,
    DateTime? EndTime,
    int? Score,
    IEnumerable<TestAttemptAnswerDto> Answers
);

public record TestAttemptAnswerDto(
    int Id,
    int AnswerId,
    DateTime AnsweredAt
);

public record SubmitTestAttemptRequest(
    int TestId,
    IEnumerable<int> AnswerIds
); 