using EduTek.Application.DTOs;

namespace EduTek.Application.Services
{
    public interface ITeacherSubjectClassService
    {
        Task<List<TeacherSubjectClassDto>> GetAllAsync();

        Task<TeacherSubjectClassDto?> GetAsync(
            int teacherId,
            int subjectId,
            int classId);

        Task<TeacherSubjectClassDto> AddAsync(
            CreateTeacherSubjectClassDto dto);

        Task<bool> DeleteAsync(
            int teacherId,
            int subjectId,
            int classId);

       
    }
}