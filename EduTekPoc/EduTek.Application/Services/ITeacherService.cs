using EduTek.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduTek.Application.Services
{
    public interface ITeacherService
    {
        Task<IEnumerable<Teacher>> GetAllAsync();
        Task<Teacher?> GetByIdAsync(int id);
        Task<Teacher> AddAsync(Teacher teacher);
        Task<bool> UpdateAsync(int id,Teacher teacher);
        Task<bool> DeleteAsync(int id);
    }
}
