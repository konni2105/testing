using EduTek.Application.DTOs;

namespace EduTek.Application.Services
{
    public interface IClassSubjectService
    {
        Task<List<ClassSubjectDto>> GetAllAsync();

        Task<ClassSubjectDto?> GetAsync(
            int classId,
            int subjectId);

        Task<ClassSubjectDto> AddAsync(
            CreateClassSubjectDto dto);

        Task<bool> DeleteAsync(
            int classId,
            int subjectId);
    }
}