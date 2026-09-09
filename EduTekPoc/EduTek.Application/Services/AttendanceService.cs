using EduTek.Application.DTOs;
using EduTek.Infrastructure.Models;
using EduTek.Infrastructure.Repositories;

namespace EduTek.Application.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _repository;
        private readonly IStudentRepository _studentRepository;
        private readonly ISubjectRepository _subjectRepository;

        public AttendanceService(
            IAttendanceRepository repository,
            IStudentRepository studentRepository,
            ISubjectRepository subjectRepository)
        {
            _repository = repository;
            _studentRepository = studentRepository;
            _subjectRepository = subjectRepository;
        }

        public async Task<List<AttendanceDto>> GetAllAsync()
        {
            var attendanceRecords =
                await _repository.GetAllAsync();

            return attendanceRecords.Select(a => new AttendanceDto
            {
                AttendanceId = a.AttendanceId,
                StudentId = a.StudentId,
                StudentName =
                    $"{a.Student.FirstName} {a.Student.LastName}",
                SubjectId = a.SubjectId,
                SubjectName = a.Subject.SubjectName,
                AttendanceDate = a.AttendanceDate,
                IsPresent = a.IsPresent
            }).ToList();
        }

        public async Task<AttendanceDto?> GetByIdAsync(int id)
        {
            var attendance =
                await _repository.GetByIdAsync(id);

            if (attendance == null)
            {
                return null;
            }

            return new AttendanceDto
            {
                AttendanceId = attendance.AttendanceId,
                StudentId = attendance.StudentId,
                StudentName =
                    $"{attendance.Student.FirstName} " +
                    $"{attendance.Student.LastName}",
                SubjectId = attendance.SubjectId,
                SubjectName = attendance.Subject.SubjectName,
                AttendanceDate = attendance.AttendanceDate,
                IsPresent = attendance.IsPresent
            };
        }

        public async Task<AttendanceDto> AddAsync(
            CreateAttendanceDto dto)
        {
            // 1. Check Student exists
            var student =
                await _studentRepository.GetByIdAsync(
                    dto.StudentId);

            if (student == null)
            {
                throw new Exception("Student not found.");
            }

            // 2. Check Subject exists
            var subject =
                await _subjectRepository.GetByIdAsync(
                    dto.SubjectId);

            if (subject == null)
            {
                throw new Exception("Subject not found.");
            }

            // 3. Check Student belongs to Class
            var studentInClass =
                await _repository.IsStudentInClassAsync(
                    dto.StudentId,
                    dto.ClassId);

            if (!studentInClass)
            {
                throw new Exception(
                    "Student does not belong to the specified class.");
            }

            // 4. Check Teacher assignment
            var teacherAssigned =
                await _repository.IsTeacherAssignedAsync(
                    dto.TeacherId,
                    dto.SubjectId,
                    dto.ClassId);

            if (!teacherAssigned)
            {
                throw new Exception(
                    "Teacher is not assigned to this subject and class.");
            }

            // 5. Check duplicate attendance
            var attendanceExists =
                await _repository.ExistsAsync(
                    dto.StudentId,
                    dto.SubjectId,
                    dto.AttendanceDate);

            if (attendanceExists)
            {
                throw new Exception(
                    "Attendance already exists for this student, subject, and date.");
            }

            // DTO → Entity
            var attendance = new Attendance
            {
                StudentId = dto.StudentId,
                SubjectId = dto.SubjectId,
                AttendanceDate = dto.AttendanceDate,
                IsPresent = dto.IsPresent
            };

            var created =
                await _repository.AddAsync(attendance);

            // Entity → DTO
            return new AttendanceDto
            {
                AttendanceId = created.AttendanceId,

                StudentId = created.StudentId,
                StudentName =
         $"{student.FirstName} {student.LastName}",

                SubjectId = created.SubjectId,
                SubjectName = subject.SubjectName,

                AttendanceDate = created.AttendanceDate,
                IsPresent = created.IsPresent
            };
        }

        public async Task<bool> UpdateAsync(
            int id,
            UpdateAttendanceDto dto)
        {
            var attendance = new Attendance
            {
                AttendanceDate = dto.AttendanceDate,
                IsPresent = dto.IsPresent
            };

            return await _repository.UpdateAsync(
                id,
                attendance);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}