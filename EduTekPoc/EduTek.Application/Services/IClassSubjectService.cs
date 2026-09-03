using EduTek.Infrastructure.Models;

namespace EduTek.Application.Services
{
    public interface IClassSubjectService
    {
        Task<List<ClassSubject>> GetAllAsync();

        Task<ClassSubject?> GetAsync(int classId, int subjectId);

        Task<ClassSubject> AddAsync(ClassSubject classSubject);

        Task<bool> DeleteAsync(int classId, int subjectId);
    }
}