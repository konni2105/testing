using EduTek.Infrastructure.Models;

namespace EduTek.Infrastructure.Repositories
{
    public interface IClassSubjectRepository
    {
        Task<ClassSubject?> GetAsync(int classId, int subjectId);

        Task<ClassSubject> AddAsync(ClassSubject classSubject);

        Task<bool> DeleteAsync(int classId, int subjectId);

        Task<List<ClassSubject>> GetAllAsync();
    }
}
