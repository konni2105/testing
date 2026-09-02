using EduTek.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduTek.Infrastructure.Repositories
{
    public interface IDepartmentRepository
    {
        Task<List<Department>> GetAllAsync();

        Task<Department?> GetByIdAsync(int id);

        Task<Department> AddAsync(Department department);

        Task<bool> UpdateAsync(int id, Department department);

        Task<bool> DeleteAsync(int id);
    }
}
