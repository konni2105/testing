using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using EduTek.Application.DTOs;
using Xunit;

namespace EduTek.API.Tests;

public class EndToEndWorkflowTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public EndToEndWorkflowTests(CustomWebApplicationFactory factory)
    {
        factory.Seed();
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task AdminJourney_ApproveUserAndCreateAcademicStructure()
    {
        await AuthenticateAsync("admin", "Admin@123");

        var register = await _client.PostAsJsonAsync("/api/Auth/register", new
        {
            Username = "pendingstudent",
            Email = "pending.student@edutek.test",
            Password = "Pending@123",
            RoleId = 3
        });
        Assert.Equal(HttpStatusCode.OK, register.StatusCode);

        var pendingResponse = await _client.GetAsync("/api/Admin/pending-registrations");
        pendingResponse.EnsureSuccessStatusCode();
        var pendingUsers = await pendingResponse.Content.ReadFromJsonAsync<List<PendingUserDto>>(JsonOptions);
        var pending = Assert.Single(pendingUsers!);

        var approve = await _client.PostAsJsonAsync("/api/Admin/approve", new
        {
            UserId = pending.UserId,
            IsApproved = true
        });
        Assert.Equal(HttpStatusCode.OK, approve.StatusCode);

        var department = await PostCreatedAsync<DepartmentDto>("/api/Department", new
        {
            DepartmentName = "Computer Science",
            Description = "CS department"
        });

        var createdClass = await PostCreatedAsync<ClassDto>("/api/Class", new
        {
            ClassName = "CS-A",
            Description = "First year"
        });

        var teacher = await PostCreatedAsync<TeacherDto>("/api/Teacher", new
        {
            FirstName = "Grace",
            LastName = "Hopper",
            Email = "grace.hopper@edutek.test",
            PhoneNumber = "9876543210"
        });

        var student = await PostCreatedAsync<StudentDto>("/api/Student", new
        {
            FirstName = "Alan",
            LastName = "Turing",
            Email = "alan.turing@edutek.test",
            PhoneNumber = "1234567890",
            DateOfBirth = new DateTime(2005, 6, 23),
            ClassId = createdClass.ClassId
        });

        var subject = await PostCreatedAsync<SubjectDto>("/api/Subject", new
        {
            SubjectName = "Algorithms",
            Description = "Core algorithms",
            DepartmentId = department.DepartmentId
        });

        var classSubject = await _client.PostAsJsonAsync("/api/ClassSubject", new
        {
            ClassId = createdClass.ClassId,
            SubjectId = subject.SubjectId
        });
        Assert.True(classSubject.IsSuccessStatusCode);

        var assignment = await _client.PostAsJsonAsync("/api/TeacherSubjectClass", new
        {
            TeacherId = teacher.TeacherId,
            SubjectId = subject.SubjectId,
            ClassId = createdClass.ClassId
        });
        Assert.True(assignment.IsSuccessStatusCode);

        var exam = await PostCreatedAsync<ExamDto>("/api/Exam", new
        {
            ExamName = "Midterm",
            SubjectId = subject.SubjectId,
            ClassId = createdClass.ClassId,
            ExamDate = DateTime.UtcNow.Date.AddDays(7)
        });

        Assert.True(department.DepartmentId > 0);
        Assert.True(createdClass.ClassId > 0);
        Assert.True(teacher.TeacherId > 0);
        Assert.True(student.StudentId > 0);
        Assert.True(exam.ExamId > 0);
    }

    [Fact]
    public async Task TeacherAndStudentJourneys_UseAssignedClassData()
    {
        await AuthenticateAsync("admin", "Admin@123");

        var department = await PostCreatedAsync<DepartmentDto>("/api/Department", new
        {
            DepartmentName = "Mathematics",
            Description = "Math department"
        });
        var createdClass = await PostCreatedAsync<ClassDto>("/api/Class", new
        {
            ClassName = "MATH-A",
            Description = "Math class"
        });
        var teacher = await PostCreatedAsync<TeacherDto>("/api/Teacher", new
        {
            FirstName = "Marie",
            LastName = "Curie",
            Email = "marie.curie@edutek.test",
            PhoneNumber = "1112223333"
        });
        var student = await PostCreatedAsync<StudentDto>("/api/Student", new
        {
            FirstName = "Ada",
            LastName = "Lovelace",
            Email = "ada.lovelace@edutek.test",
            PhoneNumber = "4445556666",
            DateOfBirth = new DateTime(2006, 1, 10),
            ClassId = createdClass.ClassId
        });
        var subject = await PostCreatedAsync<SubjectDto>("/api/Subject", new
        {
            SubjectName = "Calculus",
            Description = "Differential calculus",
            DepartmentId = department.DepartmentId
        });

        (await _client.PostAsJsonAsync("/api/ClassSubject", new
        {
            ClassId = createdClass.ClassId,
            SubjectId = subject.SubjectId
        })).EnsureSuccessStatusCode();

        (await _client.PostAsJsonAsync("/api/TeacherSubjectClass", new
        {
            TeacherId = teacher.TeacherId,
            SubjectId = subject.SubjectId,
            ClassId = createdClass.ClassId
        })).EnsureSuccessStatusCode();

        var exam = await PostCreatedAsync<ExamDto>("/api/Exam", new
        {
            ExamName = "Unit Test",
            SubjectId = subject.SubjectId,
            ClassId = createdClass.ClassId,
            ExamDate = DateTime.UtcNow.Date.AddDays(3)
        });

        await AuthenticateAsync("teacher", "Teacher@123");

        var assignedClasses = await _client.GetAsync("/api/TeacherSubjectClass");
        Assert.Equal(HttpStatusCode.OK, assignedClasses.StatusCode);

        var attendance = await _client.PostAsJsonAsync("/api/Attendance", new
        {
            TeacherId = teacher.TeacherId,
            StudentId = student.StudentId,
            SubjectId = subject.SubjectId,
            ClassId = createdClass.ClassId,
            AttendanceDate = DateTime.UtcNow.Date,
            IsPresent = true
        });
        Assert.Equal(HttpStatusCode.OK, attendance.StatusCode);

        var mark = await PostCreatedAsync<MarkDto>("/api/Mark", new
        {
            ExamId = exam.ExamId,
            StudentId = student.StudentId,
            Score = 88.5m
        });
        Assert.True(mark.MarkId > 0);

        var feedback = await PostCreatedAsync<FeedbackDto>("/api/Feedback", new
        {
            TeacherId = teacher.TeacherId,
            StudentId = student.StudentId,
            Comments = "Strong progress this week.",
            FeedbackDate = DateTime.UtcNow.Date
        });
        Assert.True(feedback.FeedbackId > 0);

        await AuthenticateAsync("student", "Student@123");

        var dashboardAttendance = await _client.GetAsync("/api/Attendance");
        var dashboardMarks = await _client.GetAsync("/api/Mark");
        var dashboardFeedback = await _client.GetAsync("/api/Feedback");
        var dashboardExams = await _client.GetAsync("/api/Exam");

        Assert.Equal(HttpStatusCode.OK, dashboardAttendance.StatusCode);
        Assert.Equal(HttpStatusCode.OK, dashboardMarks.StatusCode);
        Assert.Equal(HttpStatusCode.OK, dashboardFeedback.StatusCode);
        Assert.Equal(HttpStatusCode.OK, dashboardExams.StatusCode);

        var attendanceList = await dashboardAttendance.Content.ReadFromJsonAsync<List<AttendanceDto>>(JsonOptions);
        var marksList = await dashboardMarks.Content.ReadFromJsonAsync<List<MarkDto>>(JsonOptions);
        var feedbackList = await dashboardFeedback.Content.ReadFromJsonAsync<List<FeedbackDto>>(JsonOptions);
        var examsList = await dashboardExams.Content.ReadFromJsonAsync<List<ExamDto>>(JsonOptions);

        Assert.NotNull(attendanceList);
        Assert.Contains(attendanceList!, a => a.StudentId == student.StudentId && a.IsPresent);

        Assert.NotNull(marksList);
        Assert.Contains(marksList!, m => m.StudentId == student.StudentId && m.Score == 88.5m);

        Assert.NotNull(feedbackList);
        Assert.Contains(feedbackList!, f => f.StudentId == student.StudentId && f.Comments.Contains("Strong progress"));

        Assert.NotNull(examsList);
        Assert.Contains(examsList!, e => e.ExamName == "Unit Test");
    }

    [Fact]
    public async Task RefreshToken_CanBeRotatedAndRevokedOnLogout()
    {
        var login = await _client.PostAsJsonAsync("/api/Auth/login", new
        {
            Username = "admin",
            Password = "Admin@123"
        });
        login.EnsureSuccessStatusCode();
        var tokens = await login.Content.ReadFromJsonAsync<AuthResponseDto>(JsonOptions);
        Assert.False(string.IsNullOrWhiteSpace(tokens!.RefreshToken));

        var refresh = await _client.PostAsJsonAsync("/api/Auth/refresh-token", new
        {
            RefreshToken = tokens.RefreshToken
        });
        Assert.Equal(HttpStatusCode.OK, refresh.StatusCode);
        var refreshed = await refresh.Content.ReadFromJsonAsync<AuthResponseDto>(JsonOptions);
        Assert.False(string.IsNullOrWhiteSpace(refreshed!.Token));
        Assert.NotEqual(tokens.RefreshToken, refreshed.RefreshToken);

        var logout = await _client.PostAsJsonAsync("/api/Auth/logout", new
        {
            RefreshToken = refreshed.RefreshToken
        });
        Assert.Equal(HttpStatusCode.OK, logout.StatusCode);

        var reuse = await _client.PostAsJsonAsync("/api/Auth/refresh-token", new
        {
            RefreshToken = refreshed.RefreshToken
        });
        Assert.Equal(HttpStatusCode.Unauthorized, reuse.StatusCode);
    }

    private async Task AuthenticateAsync(string username, string password)
    {
        var login = await _client.PostAsJsonAsync("/api/Auth/login", new
        {
            Username = username,
            Password = password
        });

        login.EnsureSuccessStatusCode();
        var payload = await login.Content.ReadFromJsonAsync<AuthResponseDto>(JsonOptions);
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", payload!.Token);
    }

    private async Task<T> PostCreatedAsync<T>(string url, object body)
    {
        var response = await _client.PostAsJsonAsync(url, body);
        Assert.True(
            response.IsSuccessStatusCode,
            await response.Content.ReadAsStringAsync());

        var result = await response.Content.ReadFromJsonAsync<T>(JsonOptions);
        Assert.NotNull(result);
        return result!;
    }
}
