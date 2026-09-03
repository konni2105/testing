using EduTek.Infrastructure.Models;
using EduTek.Infrastructure.Repositories;

namespace EduTek.Application.Services
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;

        public ClassService(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public async Task<List<Class>> GetAllAsync()
        {
            return await _classRepository.GetAllAsync();
        }

        public async Task<Class?> GetByIdAsync(int id)
        {
            return await _classRepository.GetByIdAsync(id);
        }

        public async Task<Class> AddAsync(Class classEntity)
        {
            return await _classRepository.AddAsync(classEntity);
        }

        public async Task<bool> UpdateAsync(int id, Class classEntity)
        {
            return await _classRepository.UpdateAsync(id, classEntity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _classRepository.DeleteAsync(id);
        }
    }
}