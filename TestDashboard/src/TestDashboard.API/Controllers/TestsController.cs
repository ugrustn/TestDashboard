using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestDashboard.API.DTOs;
using TestDashboard.Data;
using TestDashboard.Domain.Models;

namespace TestDashboard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TestsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public TestsController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TestDto>>> GetTests()
    {
        var tests = await _context.Tests
            .Include(t => t.Creator)
            .Select(t => new TestDto(
                t.Id,
                t.Title,
                t.Description,
                t.TimeLimit,
                t.CreatedAt,
                t.CreatorId,
                $"{t.Creator.FirstName} {t.Creator.LastName}"
            ))
            .ToListAsync();

        return Ok(tests);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TestDetailsDto>> GetTest(int id)
    {
        var test = await _context.Tests
            .Include(t => t.Creator)
            .Include(t => t.Questions)
                .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (test == null)
        {
            return NotFound();
        }

        var dto = new TestDetailsDto(
            test.Id,
            test.Title,
            test.Description,
            test.TimeLimit,
            test.CreatedAt,
            test.CreatorId,
            $"{test.Creator.FirstName} {test.Creator.LastName}",
            test.Questions.Select(q => new QuestionDto(
                q.Id,
                q.Text,
                q.Points,
                q.Answers.Select(a => new AnswerDto(
                    a.Id,
                    a.Text,
                    a.IsCorrect
                ))
            ))
        );

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<TestDto>> CreateTest(CreateTestRequest request)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        var test = new Test
        {
            Title = request.Title,
            Description = request.Description,
            TimeLimit = request.TimeLimit,
            CreatorId = userId,
            Questions = request.Questions.Select(q => new Question
            {
                Text = q.Text,
                Points = q.Points,
                Answers = q.Answers.Select(a => new Answer
                {
                    Text = a.Text,
                    IsCorrect = a.IsCorrect
                }).ToList()
            }).ToList()
        };

        _context.Tests.Add(test);
        await _context.SaveChangesAsync();

        var creator = await _userManager.FindByIdAsync(userId);
        return CreatedAtAction(
            nameof(GetTest),
            new { id = test.Id },
            new TestDto(
                test.Id,
                test.Title,
                test.Description,
                test.TimeLimit,
                test.CreatedAt,
                test.CreatorId,
                $"{creator?.FirstName} {creator?.LastName}"
            )
        );
    }

    [HttpPost("{id}/attempt")]
    public async Task<ActionResult<TestAttemptDto>> StartTestAttempt(int id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        var test = await _context.Tests.FindAsync(id);
        if (test == null)
        {
            return NotFound();
        }

        var attempt = new TestAttempt
        {
            TestId = id,
            UserId = userId,
            StartTime = DateTime.UtcNow
        };

        _context.TestAttempts.Add(attempt);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetTestAttempt),
            new { id = attempt.Id },
            new TestAttemptDto(
                attempt.Id,
                attempt.TestId,
                attempt.UserId,
                attempt.StartTime,
                attempt.EndTime,
                attempt.Score,
                new List<TestAttemptAnswerDto>()
            )
        );
    }

    [HttpGet("attempts/{id}")]
    public async Task<ActionResult<TestAttemptDto>> GetTestAttempt(int id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        var attempt = await _context.TestAttempts
            .Include(ta => ta.Answers)
            .FirstOrDefaultAsync(ta => ta.Id == id && ta.UserId == userId);

        if (attempt == null)
        {
            return NotFound();
        }

        return Ok(new TestAttemptDto(
            attempt.Id,
            attempt.TestId,
            attempt.UserId,
            attempt.StartTime,
            attempt.EndTime,
            attempt.Score,
            attempt.Answers.Select(a => new TestAttemptAnswerDto(
                a.Id,
                a.AnswerId,
                a.AnsweredAt
            ))
        ));
    }

    [HttpPost("attempts/{id}/submit")]
    public async Task<ActionResult<TestAttemptDto>> SubmitTestAttempt(int id, SubmitTestAttemptRequest request)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        var attempt = await _context.TestAttempts
            .Include(ta => ta.Test)
                .ThenInclude(t => t.Questions)
                    .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(ta => ta.Id == id && ta.UserId == userId);

        if (attempt == null)
        {
            return NotFound();
        }

        if (attempt.EndTime.HasValue)
        {
            return BadRequest("Test attempt has already been submitted");
        }

        // Calculate score
        var score = 0;
        var correctAnswers = attempt.Test.Questions
            .SelectMany(q => q.Answers)
            .Where(a => a.IsCorrect)
            .Select(a => a.Id)
            .ToHashSet();

        var submittedAnswers = request.AnswerIds.ToHashSet();
        var correctSubmissions = submittedAnswers.Intersect(correctAnswers).Count();
        var incorrectSubmissions = submittedAnswers.Except(correctAnswers).Count();
        
        score = correctSubmissions * 100 / correctAnswers.Count;

        // Record answers
        var answers = request.AnswerIds.Select(answerId => new TestAttemptAnswer
        {
            TestAttemptId = id,
            AnswerId = answerId,
            AnsweredAt = DateTime.UtcNow
        });

        attempt.EndTime = DateTime.UtcNow;
        attempt.Score = score;
        _context.TestAttemptAnswers.AddRange(answers);
        await _context.SaveChangesAsync();

        return Ok(new TestAttemptDto(
            attempt.Id,
            attempt.TestId,
            attempt.UserId,
            attempt.StartTime,
            attempt.EndTime,
            attempt.Score,
            answers.Select(a => new TestAttemptAnswerDto(
                a.Id,
                a.AnswerId,
                a.AnsweredAt
            ))
        ));
    }
} 