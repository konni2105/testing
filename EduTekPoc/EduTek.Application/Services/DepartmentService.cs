using EduTek.Application.DTOs;
using EduTek.Infrastructure.Models;
using EduTek.Infrastructure.Repositories;

namespace EduTek.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(
            IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<List<DepartmentDto>> GetAllAsync()
        {
            var departments =
                await _departmentRepository.GetAllAsync();

            return departments.Select(d => new DepartmentDto
            {
                DepartmentId = d.DepartmentId,
                DepartmentName = d.DepartmentName,
                Description = d.Description
            }).ToList();
        }

        public async Task<DepartmentDto?> GetByIdAsync(int id)
        {
            var department =
                await _departmentRepository.GetByIdAsync(id);

            if (department == null)
            {
                return null;
            }

            return new DepartmentDto
            {
                DepartmentId = department.DepartmentId,
                DepartmentName = department.DepartmentName,
                Description = department.Description
            };
        }

        public async Task<DepartmentDto> AddAsync(
            CreateDepartmentDto dto)
        {
            // DTO → Entity
            var department = new Department
            {
                DepartmentName = dto.DepartmentName,
                Description = dto.Description
            };

            var createdDepartment =
                await _departmentRepository.AddAsync(department);

            // Entity → DTO
            return new DepartmentDto
            {
                DepartmentId = createdDepartment.DepartmentId,
                DepartmentName = createdDepartment.DepartmentName,
                Description = createdDepartment.Description
            };
        }

        public async Task<bool> UpdateAsync(
            int id,
            UpdateDepartmentDto dto)
        {
            // DTO → Entity
            var department = new Department
            {
                DepartmentName = dto.DepartmentName,
                Description = dto.Description
            };

            return await _departmentRepository.UpdateAsync(
                id,
                department);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _departmentRepository.DeleteAsync(id);
        }
    }
}