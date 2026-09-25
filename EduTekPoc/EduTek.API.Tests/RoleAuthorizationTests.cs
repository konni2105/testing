using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using EduTek.Application.DTOs;
using Xunit;

namespace EduTek.API.Tests;

public class RoleAuthorizationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public RoleAuthorizationTests(CustomWebApplicationFactory factory)
    {
        factory.Seed();
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ProtectedEndpoint_WithoutToken_Returns401()
    {
        var response = await _client.GetAsync("/api/Department");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Admin_CanAccessAdminOnlyApis()
    {
        await AuthenticateAsync("admin", "Admin@123");

        var pending = await _client.GetAsync("/api/Admin/pending-registrations");
        var departments = await _client.GetAsync("/api/Department");

        Assert.Equal(HttpStatusCode.OK, pending.StatusCode);
        Assert.Equal(HttpStatusCode.OK, departments.StatusCode);
    }

    [Fact]
    public async Task Teacher_IsForbiddenFromAdminOnlyApis()
    {
        await AuthenticateAsync("teacher", "Teacher@123");

        var pending = await _client.GetAsync("/api/Admin/pending-registrations");
        var createDepartment = await _client.PostAsJsonAsync("/api/Department", new
        {
            DepartmentName = "Science",
            Description = "Science department"
        });

        Assert.Equal(HttpStatusCode.Forbidden, pending.StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, createDepartment.StatusCode);
    }

    [Fact]
    public async Task Student_IsForbiddenFromTeacherAndAdminMutations()
    {
        await AuthenticateAsync("student", "Student@123");

        var students = await _client.GetAsync("/api/Student");
        var createStudent = await _client.PostAsJsonAsync("/api/Student", new
        {
            FirstName = "Ada",
            LastName = "Lovelace",
            Email = "ada@edutek.test",
            PhoneNumber = "1234567890",
            DateOfBirth = DateTime.UtcNow.AddYears(-18),
            ClassId = 1
        });
        var createAttendance = await _client.PostAsJsonAsync("/api/Attendance", new
        {
            TeacherId = 1,
            StudentId = 1,
            SubjectId = 1,
            ClassId = 1,
            AttendanceDate = DateTime.UtcNow.Date,
            IsPresent = true
        });

        Assert.Equal(HttpStatusCode.OK, students.StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, createStudent.StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, createAttendance.StatusCode);
    }

    [Fact]
    public async Task Teacher_CanReadAssignedAcademicData()
    {
        await AuthenticateAsync("teacher", "Teacher@123");

        var attendance = await _client.GetAsync("/api/Attendance");
        var marks = await _client.GetAsync("/api/Mark");
        var feedback = await _client.GetAsync("/api/Feedback");
        var assignments = await _client.GetAsync("/api/TeacherSubjectClass");

        Assert.Equal(HttpStatusCode.OK, attendance.StatusCode);
        Assert.Equal(HttpStatusCode.OK, marks.StatusCode);
        Assert.Equal(HttpStatusCode.OK, feedback.StatusCode);
        Assert.Equal(HttpStatusCode.OK, assignments.StatusCode);
    }

    [Fact]
    public async Task Student_CanReadDashboardData()
    {
        await AuthenticateAsync("student", "Student@123");

        var attendance = await _client.GetAsync("/api/Attendance");
        var marks = await _client.GetAsync("/api/Mark");
        var feedback = await _client.GetAsync("/api/Feedback");
        var exams = await _client.GetAsync("/api/Exam");

        Assert.Equal(HttpStatusCode.OK, attendance.StatusCode);
        Assert.Equal(HttpStatusCode.OK, marks.StatusCode);
        Assert.Equal(HttpStatusCode.OK, feedback.StatusCode);
        Assert.Equal(HttpStatusCode.OK, exams.StatusCode);
    }

    [Fact]
    public async Task ProtectedEndpoint_WithInvalidToken_Returns401()
    {
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", "invalid.jwt.token");

        var response = await _client.GetAsync("/api/Department");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Student_IsForbiddenFromAdminEndpoints()
    {
        await AuthenticateAsync("student", "Student@123");

        var pending = await _client.GetAsync("/api/Admin/pending-registrations");
        var createDept = await _client.PostAsJsonAsync("/api/Department", new
        {
            DepartmentName = "Forbidden Dept",
            Description = "Should fail"
        });
        var createClass = await _client.PostAsJsonAsync("/api/Class", new
        {
            ClassName = "Forbidden Class",
            Description = "Should fail"
        });

        Assert.Equal(HttpStatusCode.Forbidden, pending.StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, createDept.StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, createClass.StatusCode);
    }

    [Fact]
    public async Task Student_IsForbiddenFromCreatingMarksExamsAndFeedback()
    {
        await AuthenticateAsync("student", "Student@123");

        var createMark = await _client.PostAsJsonAsync("/api/Mark", new
        {
            ExamId = 1,
            StudentId = 1,
            Score = 95m
        });
        var createExam = await _client.PostAsJsonAsync("/api/Exam", new
        {
            ExamName = "Hacked Exam",
            SubjectId = 1,
            ClassId = 1,
            ExamDate = DateTime.UtcNow.Date
        });
        var createFeedback = await _client.PostAsJsonAsync("/api/Feedback", new
        {
            TeacherId = 1,
            StudentId = 1,
            Comments = "Self-feedback"
        });

        Assert.Equal(HttpStatusCode.Forbidden, createMark.StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, createExam.StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, createFeedback.StatusCode);
    }

    [Fact]
    public async Task Teacher_IsForbiddenFromMutatingClassesAndTeachers()
    {
        await AuthenticateAsync("teacher", "Teacher@123");

        var createClass = await _client.PostAsJsonAsync("/api/Class", new
        {
            ClassName = "Teacher Class",
            Description = "Should fail"
        });
        var createTeacher = await _client.PostAsJsonAsync("/api/Teacher", new
        {
            FirstName = "Another",
            LastName = "Teacher",
            Email = "another.teacher@edutek.test",
            PhoneNumber = "1234567890"
        });

        Assert.Equal(HttpStatusCode.Forbidden, createClass.StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, createTeacher.StatusCode);
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
        Assert.NotNull(payload);
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", payload!.Token);
    }
}
